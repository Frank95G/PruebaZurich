using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaZurich.Data.Entities;

namespace PruebaZurich.Data.Context;

public partial class ZurichDBContext : DbContext
{
    public ZurichDBContext()
    {
    }

    public ZurichDBContext(DbContextOptions<ZurichDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Poliza> Polizas { get; set; }

    public virtual DbSet<TiposPoliza> TiposPolizas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes", "seg");

            entity.HasIndex(e => e.Email, "IX_Clientes_Email");

            entity.HasIndex(e => e.Nombre, "IX_Clientes_Nombre");

            entity.HasIndex(e => e.Identificacion, "UQ_Clientes_Identificacion").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Clientes__A9D10534CADBAE0C").IsUnique();

            entity.HasIndex(e => e.Identificacion, "UQ__Clientes__D6F931E58BE7BC22").IsUnique();

            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Identificacion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Clientes_Usuarios");
        });

        modelBuilder.Entity<Poliza>(entity =>
        {
            entity.ToTable("Polizas", "seg");

            entity.HasIndex(e => e.ClienteId, "IX_Polizas_ClienteId");

            entity.HasIndex(e => e.Estado, "IX_Polizas_Estado");

            entity.HasIndex(e => new { e.FechaInicio, e.FechaExpiracion }, "IX_Polizas_Fechas");

            entity.HasIndex(e => e.NumeroPoliza, "UQ_Polizas_NumeroPoliza").IsUnique();

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Activa");
            entity.Property(e => e.FechaEmision).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.MontoAsegurado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MotivoCancelacion).HasMaxLength(255);
            entity.Property(e => e.NumeroPoliza).HasMaxLength(20);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Polizas_Clientes");

            entity.HasOne(d => d.TipoPoliza).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.TipoPolizaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Polizas_TiposPoliza");
        });

        modelBuilder.Entity<TiposPoliza>(entity =>
        {
            entity.HasKey(e => e.TipoPolizaId);

            entity.ToTable("TiposPoliza", "seg");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios", "seg");

            entity.HasIndex(e => e.Email, "UQ_Usuarios_Email").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "UQ_Usuarios_NombreUsuario").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
