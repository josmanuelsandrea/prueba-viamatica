using System;
using System.Collections.Generic;

namespace viamatica_backend.DBModels;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IntentosInicioSesion { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public char SessionActive { get; set; }

    public int IdPersona { get; set; }

    public string Status { get; set; } = null!;

    public bool Eliminado { get; set; }

    public virtual ICollection<HistorialSesione> HistorialSesiones { get; set; } = new List<HistorialSesione>();

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<RolUsuario> RolUsuarios { get; set; } = new List<RolUsuario>();

    public virtual SesionesActiva? SesionesActiva { get; set; }
}
