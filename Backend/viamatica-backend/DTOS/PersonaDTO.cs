namespace viamatica_backend.DTOS
{
    public class PersonaDTO
    {
        public int IdPersona { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Identificacion { get; set; } = null!;

        public DateOnly FechaNacimiento { get; set; }

        public bool Eliminado { get; set; }
    }
}
