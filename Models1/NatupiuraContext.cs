using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WEBAPP_NATURPIURA.Models1
{
    public partial class NatupiuraContext : DbContext
    {
        public NatupiuraContext()
        {
        }

        public NatupiuraContext(DbContextOptions<NatupiuraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Beneficio> Beneficios { get; set; } = null!;
        public virtual DbSet<CalificacionProducto> CalificacionProductos { get; set; } = null!;
        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<Detalle> Detalles { get; set; } = null!;
        public virtual DbSet<Direccion> Direccions { get; set; } = null!;
        public virtual DbSet<Envio> Envios { get; set; } = null!;
        public virtual DbSet<Kardex> Kardices { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<ProductoBeneficio> ProductoBeneficios { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Ventum> Venta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Initial Catalog=Natupiura;User ID=sa;Password=Manuel12;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beneficio>(entity =>
            {
                entity.HasKey(e => e.IdBeneficio)
                    .HasName("PK__BENEFICI__14B7CA0C2F8D1699");

                entity.ToTable("BENEFICIO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreBeneficio)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CalificacionProducto>(entity =>
            {
                entity.HasKey(e => e.IdCalificacion)
                    .HasName("PK__CALIFICA__40E4A751BDA925C0");

                entity.ToTable("CALIFICACION_PRODUCTO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCalificacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.CalificacionProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CALIFICAC__IdPro__00200768");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.CalificacionProductos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CALIFICAC__IdUsu__7F2BE32F");
            });

            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.IdCarrito)
                    .HasName("PK__CARRITO__8B4A618CC7855516");

                entity.ToTable("CARRITO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CARRITO__IdUsuar__797309D9");
            });

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__CATEGORI__A3C02A10E36D8CF3");

                entity.ToTable("CATEGORIA");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Detalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalles)
                    .HasName("PK__DETALLES__963DC03980C110AB");

                entity.ToTable("DETALLES");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('pendiente de pago')");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PrecioUnidad)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Precio Unidad");

                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdCarritoNavigation)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(d => d.IdCarrito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DETALLES__IdCarr__6EF57B66");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DETALLES__IdProd__6D0D32F4");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(d => d.IdVenta)
                    .HasConstraintName("FK__DETALLES__IdVent__6E01572D");
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasKey(e => e.IdDireccion)
                    .HasName("PK__Direccio__1F8E0C76B7C073A5");

                entity.ToTable("Direccion");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Calle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Distrito)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Provincia)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Direccion__IdUsu__74AE54BC");
            });

            modelBuilder.Entity<Envio>(entity =>
            {
                entity.HasKey(e => e.IdEnvio)
                    .HasName("PK__ENVIO__B814A62EFC7BF976");

                entity.ToTable("ENVIO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.CodigoSeguimiento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoEnvio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Pendiente')");

                entity.Property(e => e.FechaEnvio).HasColumnType("datetime");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.Envios)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ENVIO__IdVenta__7E37BEF6");
            });

            modelBuilder.Entity<Kardex>(entity =>
            {
                entity.HasKey(e => e.IdKardex)
                    .HasName("PK__KARDEX__BC1BA40081615069");

                entity.ToTable("KARDEX");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CostoUnitario).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DocumentoReferencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaMovimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroSistema)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoCantidad).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SaldoCosto).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalCostoMovimiento).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Kardices)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KARDEX__IdProduc__7B5B524B");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Kardices)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KARDEX__IdUsuari__7A672E12");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.IdPago)
                    .HasName("PK__PAGO__FC851A3AE256D8B6");

                entity.ToTable("PAGO");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Metodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tarjeta)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__PAGO__IdUsuario__76969D2E");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK__PRODUCTO__09889210CFE284E2");

                entity.ToTable("PRODUCTO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagenUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioUnidadCompra).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrecioUnidadVenta).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Stock).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__PRODUCTO__IdCate__75A278F5");
            });

            modelBuilder.Entity<ProductoBeneficio>(entity =>
            {
                entity.HasKey(e => new { e.IdProducto, e.IdBeneficio })
                    .HasName("PK__PRODUCTO__D8C3EEB01C684078");

                entity.ToTable("PRODUCTO_BENEFICIO");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdBeneficioNavigation)
                    .WithMany(p => p.ProductoBeneficios)
                    .HasForeignKey(d => d.IdBeneficio)
                    .HasConstraintName("FK__PRODUCTO___IdBen__7D439ABD");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.ProductoBeneficios)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__PRODUCTO___IdPro__7C4F7684");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__ROL__2A49584C143C86B3");

                entity.ToTable("ROL");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Permisos)
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__USUARIO__5B65BF97600DFF99");

                entity.ToTable("USUARIO");

                entity.HasIndex(e => e.Clave, "IX_Clave");

                entity.HasIndex(e => e.Correo, "IX_Correo");

                entity.HasIndex(e => e.Correo, "UQ__USUARIO__60695A19E446C22F")
                    .IsUnique();

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Clave).HasMaxLength(64);

                entity.Property(e => e.Correo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__USUARIO__IdRol__73BA3083");
            });

            modelBuilder.Entity<Ventum>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PK__VENTA__BC1240BDD4A2D618");

                entity.ToTable("VENTA");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalCosto).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UrlSustento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPagoNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdPago)
                    .HasConstraintName("FK__VENTA__IdPago__787EE5A0");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__VENTA__IdUsuario__778AC167");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
