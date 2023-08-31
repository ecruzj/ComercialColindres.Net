using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CategoriaProductosMap : EntityTypeConfiguration<CategoriaProductos>
    {
        public CategoriaProductosMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoriaProductoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CategoriaProductos");
            this.Property(t => t.CategoriaProductoId).HasColumnName("CategoriaProductoId");
            this.Property(t => t.BiomasaId).HasColumnName("BiomasaId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");

            // Relationships
            this.HasRequired(t => t.MaestroBiomasa)
                .WithMany(t => t.CategoriaProductos)
                .HasForeignKey(d => d.BiomasaId);
        }
    }
}
