using System.Net;
using AutoMapper;
using viamatica_backend.DBModels;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;
using viamatica_backend.Models.Utility;
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

        public async Task<APIResponse<UsuarioDTO>> WhoAmI(TokenRequest data)
        {
            var usuario = await _sesionesActivasService.ObtenerUsuarioPorToken(data.Token);
            if (usuario == null)
            {
                return new APIResponse<UsuarioDTO>(null, "Token expirado, o invalido", HttpStatusCode.NotFound);
            }
            var response = _mapper.Map<UsuarioDTO>(usuario);

            var roles = await _rolUsuarioRepository.GetFilteredAsync(r => r.IdUsuario == usuario.IdUsuario) ?? new List<RolUsuario>();
            var permisos = await _rolOpcionesRepository.GetFilteredAsync(ro => roles.Select(r => r.IdRol).Contains(ro.IdRol)) ?? new List<RolOpcione>();

            response.Roles = _mapper.Map<IEnumerable<RoleUserDTO>>(roles);
            response.Permisos = _mapper.Map<IEnumerable<OpcionesDTO>>(permisos);

            return new APIResponse<UsuarioDTO>(response, "Usuario loggeado", HttpStatusCode.OK);
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
        public async Task<APIResponse<bool>> Logout(TokenRequest token)
        {
            // Buscar la sesión activa en la base de datos
            var sesion = await _sesionesActivasService.ObtenerSesionPorToken(token.Token);

            if (sesion == null)
            {
                return new APIResponse<bool>(false, "Sesión no encontrada o ya cerrada", HttpStatusCode.NotFound);
            }

            // Buscar el usuario asociado a la sesión
            var usuario = await _usuarioRepository.GetByIdAsync(sesion.IdUsuario);
            if (usuario == null)
            {
                return new APIResponse<bool>(false, "Usuario no encontrado", HttpStatusCode.NotFound);
            }

            // Marcar la sesión como finalizada
            await _sesionesActivasService.EliminarSesion(sesion.IdUsuario);

            // Actualizar estado de sesión en la tabla usuarios
            usuario.SessionActive = 'F';
            await _usuarioRepository.UpdateAsync(usuario);

            // Registrar el cierre de sesión en el historial
            await _historialSesionesService.RegistrarHistorial(usuario.IdUsuario, true, null);

            return new APIResponse<bool>(true, "Cierre de sesión exitoso", HttpStatusCode.OK);
        }

    }
}
