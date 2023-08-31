using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class DeduccionesMap : EntityTypeConfiguration<Deducciones>
    {
        public DeduccionesMap()
        {
            // Primary Key
            this.HasKey(t => t.DeduccionId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Deducciones");
            this.Property(t => t.DeduccionId).HasColumnName("DeduccionId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
