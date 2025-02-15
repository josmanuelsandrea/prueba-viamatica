using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class RolOpcione
{
    public int IdRol { get; set; }

    public int IdOpcion { get; set; }

    public bool Eliminado { get; set; }

    public virtual Opcione IdOpcionNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
