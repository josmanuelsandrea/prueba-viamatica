using AutoMapper;
using viamatica_backend.DBModels;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;
using viamatica_backend.Repository;
using viamatica_backend.Tools;

namespace viamatica_backend.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SesionesActivasService _sesionesActivasService;
        private readonly HistorialSesionesService _historialSesionesService;
        private readonly RolUsuarioRepository _rolUsuarioRepository;
        private readonly RolOpcionesRepository _rolOpcionesRepository;
        private readonly IMapper _mapper;
        public AuthService(UsuarioService usuarioService, UsuarioRepository usuarioRepository, SesionesActivasService sesionesActivasService, HistorialSesionesService historialSesionesService, RolUsuarioRepository rolUsuarioRepository, RolOpcionesRepository rolOpcionesRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _sesionesActivasService = sesionesActivasService;
            _historialSesionesService = historialSesionesService;
            _rolUsuarioRepository = rolUsuarioRepository;
            _rolOpcionesRepository = rolOpcionesRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponseDTO> Login(UserLoginData data)
        {
            Usuario? foundUser = null;

            // Buscar usuario por email o username
            if (!string.IsNullOrEmpty(data.Email) || !string.IsNullOrEmpty(data.Username))
            {
                var usuarios = await _usuarioRepository.GetFilteredAsync(u =>
                    u.Email == data.Email || u.Username == data.Username);
                foundUser = usuarios.FirstOrDefault();
            }

            // Si el usuario no existe
            if (foundUser == null)
            {
                return new LoginResponseDTO { Success = false, Message = "Usuario no encontrado" };
            }

            // Verificar si el usuario está bloqueado por intentos fallidos
            if (foundUser.IntentosInicioSesion >= 5)
            {
                return new LoginResponseDTO { Success = false, Message = "Cuenta bloqueada por múltiples intentos fallidos" };
            }

            // Validar la contraseña
            bool isPasswordValid = PasswordHasher.VerifyPassword(data.Password, foundUser.Password);

            if (!isPasswordValid)
            {
                foundUser.IntentosInicioSesion += 1;
                await _usuarioRepository.UpdateAsync(foundUser);
                await _historialSesionesService.RegistrarHistorial(foundUser.IdUsuario, false, null);

                return new LoginResponseDTO { Success = false, Message = "Credenciales incorrectas" };
            }

            // Restablecer intentos fallidos
            foundUser.IntentosInicioSesion = 0;
            foundUser.SessionActive = 'T';
            await _usuarioRepository.UpdateAsync(foundUser);

            // Generar token de sesión
            string token = TokenGenerator.GenerateToken(foundUser.IdUsuario); // 🔹 Corregido: usar IdUsuario

            // Registrar sesión activa
            await _sesionesActivasService.RegistrarSesionActiva(foundUser.IdUsuario, token);

            // Registrar en historial de sesiones
            await _historialSesionesService.RegistrarHistorial(foundUser.IdUsuario, true, token);

            // Obtener roles y permisos del usuario
            var roles = await _rolUsuarioRepository.GetFilteredAsync(r => r.IdUsuario == foundUser.IdUsuario) ?? new List<RolUsuario>();
            var permisos = await _rolOpcionesRepository.GetFilteredAsync(ro => roles.Select(r => r.IdRol).Contains(ro.IdRol)) ?? new List<RolOpcione>();


            UsuarioDTO UserResponse = _mapper.Map<UsuarioDTO>(foundUser);


            UserResponse.Roles = _mapper.Map<IEnumerable<RoleUserDTO>>(roles);
            UserResponse.Permisos = _mapper.Map<IEnumerable<OpcionesDTO>>(permisos);
            return new LoginResponseDTO
            {
                Success = true,
                Message = "Inicio de sesión exitoso",
                Token = token,
                User = UserResponse
            };
        }
    }
}
