using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAppPixca.Models;

public partial class PixcaContext : DbContext
{
    public PixcaContext()
    {
    }

    public PixcaContext(DbContextOptions<PixcaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Envio> Envios { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
optionsBuilder.UseMySql("Server=MYSQL5045.site4now.net;Database=db_a99a85_epixcan;Uid=a99a85_epixcan;Pwd=Tt7Xfhb@HL_iKB*", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PRIMARY");

            entity.ToTable("carrito");

            entity.HasIndex(e => e.IdProduct, "IdProduct");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Fecha).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrito_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrito_ibfk_2");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.NombreCategoria).HasMaxLength(25);
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.IdEnvio).HasName("PRIMARY");

            entity.ToTable("envio");

            entity.HasIndex(e => e.IdPago, "IdPago");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Calle).HasMaxLength(60);
            entity.Property(e => e.CodigoPostal).HasMaxLength(10);
            entity.Property(e => e.Localidad).HasMaxLength(60);
            entity.Property(e => e.Municipio).HasMaxLength(70);
            entity.Property(e => e.NumeroInterior).HasMaxLength(40);
            entity.Property(e => e.Referencia1).HasMaxLength(60);
            entity.Property(e => e.Referencia2).HasMaxLength(60);

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Envios)
                .HasForeignKey(d => d.IdPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("envio_ibfk_2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Envios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("envio_ibfk_1");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PRIMARY");

            entity.ToTable("pago");

            entity.HasIndex(e => e.IdVenta, "IdVenta");

            entity.Property(e => e.Efectivo).HasMaxLength(20);
            entity.Property(e => e.Tarjeta).HasMaxLength(20);
            entity.Property(e => e.Trasferencia).HasMaxLength(25);

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pago_ibfk_1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdCategoria, "IdCategoria");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.Imagen).HasColumnType("blob");
            entity.Property(e => e.NombreProduct).HasMaxLength(30);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_ibfk_2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.ApellidoMater).HasMaxLength(40);
            entity.Property(e => e.ApellidoPater).HasMaxLength(40);
            entity.Property(e => e.Contraseña).HasMaxLength(10);
            entity.Property(e => e.Curp).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.NombreUsuario).HasMaxLength(40);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(11);
            entity.Property(e => e.Rfc).HasMaxLength(20);
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PRIMARY");

            entity.ToTable("venta");

            entity.HasIndex(e => e.IdProduct, "IdProduct");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Fecha).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("venta_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("venta_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
