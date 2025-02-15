using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class SesionesActiva
{
    public int IdSesion { get; set; }

    public int IdUsuario { get; set; }

    public string Token { get; set; } = null!;

    public DateTime FechaExpiracion { get; set; }

    public DateTime FechaInicio { get; set; }

    public bool Eliminado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
