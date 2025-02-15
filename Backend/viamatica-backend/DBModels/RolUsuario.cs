using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class RolUsuario
{
    public int IdRol { get; set; }

    public int IdUsuario { get; set; }

    public bool Eliminado { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
