namespace viamatica_backend.DTOS
{
    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UsuarioDTO User { get; set; }
    }
}
