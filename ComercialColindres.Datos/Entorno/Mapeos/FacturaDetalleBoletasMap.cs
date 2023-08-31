using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FacturaDetalleBoletasMap : EntityTypeConfiguration<FacturaDetalleBoletas>
    {
        public FacturaDetalleBoletasMap()
        {
            // Primary Key
            this.HasKey(t => t.FacturaDetalleBoletaId);
            
            // Table & Column Mappings
            this.ToTable("FacturaDetalleBoletas");
            this.Property(t => t.FacturaDetalleBoletaId).HasColumnName("FacturaDetalleBoletaId");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.NumeroEnvio).HasColumnName("NumeroEnvio");
            this.Property(t => t.CodigoBoleta).HasColumnName("CodigoBoleta");
            this.Property(t => t.EstaIngresada).HasColumnName("EstaIngresada");
            this.Property(t => t.PesoProducto).HasColumnName("PesoProducto");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.FechaIngreso).HasColumnName("FechaIngreso");

            // Relationships
            this.HasOptional(t => t.Boleta)
                .WithMany(t => t.FacturaDetalleBoletas)
                .HasForeignKey(d => d.BoletaId);
            this.HasRequired(t => t.Factura)
                .WithMany(t => t.FacturaDetalleBoletas)
                .HasForeignKey(d => d.FacturaId);
            this.HasRequired(t => t.Planta)
                .WithMany(t => t.FacturaDetalleBoletas)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
