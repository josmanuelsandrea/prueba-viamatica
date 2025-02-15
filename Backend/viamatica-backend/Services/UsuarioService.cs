using System.Diagnostics;
using System.Net;
using AutoMapper;
using viamatica_backend.DBModels;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Enums;
using viamatica_backend.Models.Request;
using viamatica_backend.Models.Utility;
using viamatica_backend.Repository;
using viamatica_backend.Tools;

namespace viamatica_backend.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PersonaRepository _personaRepository;
        private readonly RolUsuarioRepository _rolUsuarioRepository;
        private readonly RolRepository _rolRepository;
        private readonly ViamaticaContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(UsuarioRepository usuarioRepository, PersonaRepository personaRepository, RolUsuarioRepository rolUsuarioRepository, RolRepository rolRepository, ViamaticaContext context, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _personaRepository = personaRepository;
            _rolUsuarioRepository = rolUsuarioRepository;
            _rolRepository = rolRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<UsuarioDTO>> ObtenerUsuarioPorId(int id)
        {
            var result = await _usuarioRepository.GetByIdAsync(id);
            var resultResponse = _mapper.Map<UsuarioDTO>(result);

            return new APIResponse<UsuarioDTO>(resultResponse, "Usuario encontrado", HttpStatusCode.OK);
        }

        public async Task<APIResponse<UsuarioDTO?>> CrearUsuario(PersonaRequest newUserData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool identificacionExiste = await _personaRepository.ExistsAsync(p => p.Identificacion == newUserData.Identificacion);
                if (identificacionExiste)
                {
                    return new APIResponse<UsuarioDTO?>(null, "La identificación ya está registrada.", HttpStatusCode.Conflict);
                }

                bool usuarioExiste = await _usuarioRepository.ExistsAsync(u => u.Username == newUserData.UserName);
                if (usuarioExiste)
                {
                    return new APIResponse<UsuarioDTO?>(null, "Nombre de usuario ya existe", HttpStatusCode.Conflict);
                }


                // Mapeo de PersonaRequest a Persona
                var newPersonData = _mapper.Map<Persona>(newUserData);

                // Guardar Persona en la BD
                var personaCreada = await _personaRepository.AddAsync(newPersonData);

                // Generar un correo único
                string generatedMail;
                int contador = 0;

                do
                {
                    generatedMail = MailGeneration.GenerateEmail(newPersonData.Nombres, newPersonData.Apellidos, contador > 0 ? contador : (int?)null);
                    contador++;
                }
                while (await _usuarioRepository.ExistsAsync(u => u.Email == generatedMail));

                // Crear usuario con datos relacionados
                var newUser = new Usuario
                {
                    IdPersona = personaCreada.IdPersona, // Relación con Persona
                    Email = generatedMail,
                    Password = PasswordHasher.HashPassword(newUserData.Contrasena),
                    Username = newUserData.UserName,
                    Status = UserStatus.ENABLED
                };

                // Guardar Usuario en la BD
                var usuarioCreado = await _usuarioRepository.AddAsync(newUser);
                var userRoleSearch = await _rolRepository.GetFilteredAsync(x => x.NombreRol == "Usuario");
                var userRoleId = userRoleSearch.FirstOrDefault();

                if (userRoleId == null)
                {
                    throw new Exception("No se ha encontrado el rol 'Usuario' en la base de datos");
                }

                var roleAssignment = new RolUsuario
                {
                    IdRol = userRoleId.IdRol,
                    IdUsuario = usuarioCreado.IdUsuario
                };


                var roleAssignmentResult = await _rolUsuarioRepository.AddAsync(roleAssignment);

                // Confirmar transacción
                await transaction.CommitAsync();

                // Mapear a DTO para la respuesta
                var usuarioDTO = _mapper.Map<UsuarioDTO>(usuarioCreado);

                return new APIResponse<UsuarioDTO?>(usuarioDTO, "Usuario creado exitosamente", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                // Revertir cambios en caso de error
                await transaction.RollbackAsync();
                return new APIResponse<UsuarioDTO?>(null, $"Error: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<APIResponse<IEnumerable<UsuarioDTO>>> ObtenerUsuariosComunes()
        {
            var roles = await _rolRepository.GetFilteredAsync(x => x.NombreRol == "Usuario");
            var foundRole = roles.FirstOrDefault();
            var usersFound = await _rolUsuarioRepository.GetFilteredAsync(x => x.IdRol == foundRole.IdRol);

            // Extraer la propiedad IdUsuarioNavigation de cada elemento
            var users = usersFound.Select(x => x.IdUsuarioNavigation).Where(u => u != null);

            var response = _mapper.Map<IEnumerable<UsuarioDTO>>(users);
            return new APIResponse<IEnumerable<UsuarioDTO>>(response, "Usuarios encontrados", HttpStatusCode.OK);
        }
    }
}
