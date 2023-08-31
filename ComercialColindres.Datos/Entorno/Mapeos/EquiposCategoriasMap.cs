using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class EquiposCategoriasMap : EntityTypeConfiguration<EquiposCategorias>
    {
        public EquiposCategoriasMap()
        {
            // Primary Key
            this.HasKey(t => t.EquipoCategoriaId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("EquiposCategorias");
            this.Property(t => t.EquipoCategoriaId).HasColumnName("EquipoCategoriaId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
