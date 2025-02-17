using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class Opcione
{
    public int IdOpcion { get; set; }

    public string NombreOpcion { get; set; } = null!;

    public bool Eliminado { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<RolOpcione> RolOpciones { get; set; } = new List<RolOpcione>();
}
