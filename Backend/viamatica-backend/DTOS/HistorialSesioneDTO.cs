namespace viamatica_backend.DTOS
{
    public class HistorialSesioneDTO
    {
        public int IdHistorial { get; set; }

        public int IdUsuario { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaCierre { get; set; }

        public bool Exito { get; set; }

        public string? Token { get; set; }

        public bool Eliminado { get; set; }

    }
}
