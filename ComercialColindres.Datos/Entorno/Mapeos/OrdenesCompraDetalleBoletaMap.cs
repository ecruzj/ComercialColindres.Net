using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OrdenesCompraDetalleBoletaMap : EntityConfiguration<OrdenesCompraDetalleBoleta>
    {
        public OrdenesCompraDetalleBoletaMap()
        {
            // Primary Key
            this.HasKey(t => t.OrdenCompraDetalleBoletaId);

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
            this.ToTable("OrdenesCompraDetalleBoleta");
            this.Property(t => t.OrdenCompraDetalleBoletaId).HasColumnName("OrdenCompraDetalleBoletaId");
            this.Property(t => t.OrdenCompraProductoId).HasColumnName("OrdenCompraProductoId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.CantidadFacturada).HasColumnName("CantidadFacturada");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.OrdenesCompraDetalleBoletas)
                .HasForeignKey(d => d.BoletaId);
            this.HasRequired(t => t.OrdenesCompraProducto)
                .WithMany(t => t.OrdenesCompraDetalleBoletas)
                .HasForeignKey(d => d.OrdenCompraProductoId);
        }
    }
}
