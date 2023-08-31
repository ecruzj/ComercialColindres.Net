using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class AjusteTipoMap : EntityTypeConfiguration<AjusteTipo>
    {
        public AjusteTipoMap()
        {
            // Primary Key
            this.HasKey(t => t.AjusteTipoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AjusteTipos");
            this.Property(t => t.AjusteTipoId).HasColumnName("AjusteTipoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.UseCreditLine).HasColumnName("UseCreditLine");
        }
    }
}
