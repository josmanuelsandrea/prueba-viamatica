namespace viamatica_backend.Models.Request
{
    public class UserLoginData
    {
        // Usuario puede iniciar sesion tanto con username como con email
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;        
    }
}
