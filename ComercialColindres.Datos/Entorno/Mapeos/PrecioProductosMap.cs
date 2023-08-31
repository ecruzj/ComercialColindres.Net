using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PrecioProductosMap : EntityConfiguration<PrecioProductos>
    {
        public PrecioProductosMap()
        {
            // Primary Key
            this.HasKey(t => t.PrecioProductoId);

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
            this.ToTable("PrecioProductos");
            this.Property(t => t.PrecioProductoId).HasColumnName("PrecioProductoId");
            this.Property(t => t.CategoriaProductoId).HasColumnName("CategoriaProductoId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.PrecioCompra).HasColumnName("PrecioCompra");
            this.Property(t => t.PrecioVenta).HasColumnName("PrecioVenta");
            this.Property(t => t.EsPrecioActual).HasColumnName("EsPrecioActual");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.CategoriaProducto)
                .WithMany(t => t.PrecioProductos)
                .HasForeignKey(d => d.CategoriaProductoId);
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.PrecioProductos)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
