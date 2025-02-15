using viamatica_backend.DBModels;

namespace viamatica_backend.DTOS
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public int? IntentosInicioSesion { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public char SessionActive { get; set; }
        public int IdPersona { get; set; }
        public string Status { get; set; } = null!;
        public bool Eliminado { get; set; }
        public PersonaDTO? Persona { get; set; }
        public IEnumerable<RoleUserDTO>? Roles { get; set; }
        public IEnumerable<OpcionesDTO>? Permisos { get; set; }
    }
}
