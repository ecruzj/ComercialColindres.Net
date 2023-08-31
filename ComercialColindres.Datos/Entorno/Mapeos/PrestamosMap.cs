using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PrestamosMap : EntityConfiguration<Prestamos>
    {
        public PrestamosMap()
        {
            // Primary Key
            this.HasKey(t => t.PrestamoId);

            // Properties
            this.Property(t => t.CodigoPrestamo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AutorizadoPor)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.TipoTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModificadoPor)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.TransaccionUId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Prestamos");
            this.Property(t => t.PrestamoId).HasColumnName("PrestamoId");
            this.Property(t => t.CodigoPrestamo).HasColumnName("CodigoPrestamo");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.AutorizadoPor).HasColumnName("AutorizadoPor");
            this.Property(t => t.PorcentajeInteres).HasColumnName("PorcentajeInteres");
            this.Property(t => t.EsInteresMensual).HasColumnName("EsInteresMensual");
            this.Property(t => t.MontoPrestamo).HasColumnName("MontoPrestamo");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.Prestamos)
                .HasForeignKey(d => d.ProveedorId);
            this.HasRequired(t => t.Sucursal)
                .WithMany(t => t.Prestamos)
                .HasForeignKey(d => d.SucursalId);
        }
    }

}
