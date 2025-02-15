using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class Rol
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public bool Eliminado { get; set; }

    public virtual ICollection<RolOpcione> RolOpciones { get; set; } = new List<RolOpcione>();

    public virtual ICollection<RolUsuario> RolUsuarios { get; set; } = new List<RolUsuario>();
}
