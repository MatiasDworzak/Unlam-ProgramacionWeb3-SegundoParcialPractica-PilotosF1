using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionF1.Data.Entidades;

public partial class GestionF1Context : DbContext
{
    public GestionF1Context()
    {
    }

    public GestionF1Context(DbContextOptions<GestionF1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Escuderium> Escuderia { get; set; }

    public virtual DbSet<Piloto> Pilotos { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Escuderium>(entity =>
        {
            entity.HasKey(e => e.IdEscuderia).HasName("PK__Escuderi__E948A0DC7D1152BD");

            entity.Property(e => e.NombreEscuderia).HasMaxLength(50);
        });

        modelBuilder.Entity<Piloto>(entity =>
        {
            entity.HasKey(e => e.IdPiloto).HasName("PK__Piloto__DB35379FB50F321C");

            entity.ToTable("Piloto");

            entity.Property(e => e.NombrePiloto).HasMaxLength(50);

            entity.HasOne(d => d.IdEscuderiaNavigation).WithMany(p => p.Pilotos)
                .HasForeignKey(d => d.IdEscuderia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Piloto__IdEscude__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
