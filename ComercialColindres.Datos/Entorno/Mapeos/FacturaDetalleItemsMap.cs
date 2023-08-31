using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FacturaDetalleItemsMap : EntityTypeConfiguration<FacturaDetalleItem>
    {
        public FacturaDetalleItemsMap()
        {
            //Primary Key
            this.HasKey(t => t.FacturaDetalleItemId);

            //Properties
            this.Property(t => t.FacturaId)
                .IsRequired();

            this.Property(t => t.Cantidad)
                .IsRequired();

            this.Property(t => t.CategoriaProductoId)
                .IsRequired();

            this.Property(t => t.Precio)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("FacturaDetalleItems");
            this.Property(t => t.FacturaDetalleItemId).HasColumnName("FacturaDetalleItemId");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.Cantidad).HasColumnName("Cantidad");
            this.Property(t => t.CategoriaProductoId).HasColumnName("CategoriaProductoId");
            this.Property(t => t.Precio).HasColumnName("Precio");

            // Relationships
            this.HasRequired(t => t.Factura)
              .WithMany(t => t.FacturaDetalleItems)
              .HasForeignKey(d => d.FacturaId);
            this.HasRequired(t => t.CategoriaProducto)
             .WithMany(t => t.FacturaDetalleItems)
             .HasForeignKey(d => d.CategoriaProductoId);
        }
    }
}
