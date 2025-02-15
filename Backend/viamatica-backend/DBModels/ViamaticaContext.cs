using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace viamatica_backend.DBModels;

public partial class ViamaticaContext : DbContext
{
    public ViamaticaContext()
    {
    }

    public ViamaticaContext(DbContextOptions<ViamaticaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HistorialSesione> HistorialSesiones { get; set; }

    public virtual DbSet<Opcione> Opciones { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolOpcione> RolOpciones { get; set; }

    public virtual DbSet<RolUsuario> RolUsuarios { get; set; }

    public virtual DbSet<SesionesActiva> SesionesActivas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistorialSesione>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("historial_sesiones_pkey");

            entity.ToTable("historial_sesiones");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.Exito).HasColumnName("exito");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialSesiones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_historial_sesiones_usuario");
        });

        modelBuilder.Entity<Opcione>(entity =>
        {
            entity.HasKey(e => e.IdOpcion).HasName("opciones_pkey");

            entity.ToTable("opciones");

            entity.Property(e => e.IdOpcion).HasColumnName("id_opcion");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.NombreOpcion)
                .HasMaxLength(50)
                .HasColumnName("nombre_opcion");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("persona_pkey");

            entity.ToTable("persona");

            entity.HasIndex(e => e.Identificacion, "persona_identificacion_key").IsUnique();

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(80)
                .HasColumnName("apellidos");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(10)
                .HasColumnName("identificacion");
            entity.Property(e => e.Nombres)
                .HasMaxLength(80)
                .HasColumnName("nombres");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("rol_pkey");

            entity.ToTable("rol");

            entity.HasIndex(e => e.NombreRol, "rol_nombre_rol_key").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<RolOpcione>(entity =>
        {
            entity.HasKey(e => new { e.IdRol, e.IdOpcion }).HasName("rol_opciones_pkey");

            entity.ToTable("rol_opciones");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdOpcion).HasColumnName("id_opcion");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");

            entity.HasOne(d => d.IdOpcionNavigation).WithMany(p => p.RolOpciones)
                .HasForeignKey(d => d.IdOpcion)
                .HasConstraintName("fk_rol_opciones_rel_opcion");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolOpciones)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("fk_rol_opciones_rel_rol");
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => new { e.IdRol, e.IdUsuario }).HasName("rol_usuarios_pkey");

            entity.ToTable("rol_usuarios");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolUsuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("fk_rol_usuarios_rol");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RolUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_rol_usuarios_usuario");
        });

        modelBuilder.Entity<SesionesActiva>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("sesiones_activas_pkey");

            entity.ToTable("sesiones_activas");

            entity.HasIndex(e => e.IdUsuario, "sesiones_activas_id_usuario_key").IsUnique();

            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.FechaExpiracion).HasColumnName("fecha_expiracion");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.SesionesActiva)
                .HasForeignKey<SesionesActiva>(d => d.IdUsuario)
                .HasConstraintName("fk_sesiones_activas_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "usuarios_username_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Eliminado)
                .HasDefaultValue(false)
                .HasColumnName("eliminado");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .HasColumnName("email");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.IntentosInicioSesion)
                .HasDefaultValue(0)
                .HasColumnName("intentos_inicio_sesion");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.SessionActive)
                .HasMaxLength(1)
                .HasColumnName("session_active");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdPersona)
                .HasConstraintName("fk_usuarios_persona");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
