namespace viamatica_backend.Models.Request
{
    public class PersonaUpdateData
    {
        public int IdPersona { get; set; }
        public string? Nombres { get; set; } = null!;
        public string? Apellidos { get; set; } = null!;
    }
}
