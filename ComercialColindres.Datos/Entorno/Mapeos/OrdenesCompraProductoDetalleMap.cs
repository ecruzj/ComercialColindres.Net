using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OrdenesCompraProductoDetalleMap : EntityConfiguration<OrdenesCompraProductoDetalle>
    {
        public OrdenesCompraProductoDetalleMap()
        {
            // Primary Key
            this.HasKey(t => t.OrdenCompraProductoDetalleId);

            // Properties
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
            this.ToTable("OrdenesCompraProductoDetalle");
            this.Property(t => t.OrdenCompraProductoDetalleId).HasColumnName("OrdenCompraProductoDetalleId");
            this.Property(t => t.OrdenCompraProductoId).HasColumnName("OrdenCompraProductoId");
            this.Property(t => t.BiomasaId).HasColumnName("BiomasaId");
            this.Property(t => t.Toneladas).HasColumnName("Toneladas");
            this.Property(t => t.PrecioDollar).HasColumnName("PrecioDollar");
            this.Property(t => t.PrecioLps).HasColumnName("PrecioLps").HasPrecision(18, 10);
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.MaestroBiomasa)
                .WithMany(t => t.OrdenesCompraProductoDetalles)
                .HasForeignKey(d => d.BiomasaId);
            this.HasRequired(t => t.OrdenesCompraProducto)
                .WithMany(t => t.OrdenesCompraProductoDetalles)
                .HasForeignKey(d => d.OrdenCompraProductoId);

        }
    }
}
