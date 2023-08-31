using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FacturasCategoriasMap : EntityTypeConfiguration<FacturasCategorias>
    {
        public FacturasCategoriasMap()
        {
            // Primary Key
            this.HasKey(t => t.FacturaCategoriaId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FacturasCategorias");
            this.Property(t => t.FacturaCategoriaId).HasColumnName("FacturaCategoriaId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
