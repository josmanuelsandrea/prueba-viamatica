using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class HistorialSesione
{
    public int IdHistorial { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaCierre { get; set; }

    public bool Exito { get; set; }

    public string? Token { get; set; }

    public bool Eliminado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
