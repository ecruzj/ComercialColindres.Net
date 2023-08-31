using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BonificacionProductoMap : EntityTypeConfiguration<BonificacionProducto>
    {
        public BonificacionProductoMap()
        {
            // Primary Key
            this.HasKey(t => t.BonificacionId);

            // Properties
            this.Property(t => t.PlantaId)
                .IsRequired();

            this.Property(t => t.CategoriaProductoId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BonificacionProducto");
            this.Property(t => t.BonificacionId).HasColumnName("BonificacionId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.CategoriaProductoId).HasColumnName("CategoriaProductoId");
            this.Property(t => t.IsEnable).HasColumnName("IsEnable");

            // Relationships
            this.HasRequired(t => t.CategoriaProducto)
                .WithMany(t => t.BonificacionesProducto)
                .HasForeignKey(d => d.CategoriaProductoId);
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.BonificacionesProducto)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
