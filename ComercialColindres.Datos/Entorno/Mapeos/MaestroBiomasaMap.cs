using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class MaestroBiomasaMap : EntityTypeConfiguration<MaestroBiomasa>
    {
        public MaestroBiomasaMap()
        {
            // Primary Key
            this.HasKey(t => t.BiomasaId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("MaestroBiomasa");
            this.Property(t => t.BiomasaId).HasColumnName("BiomasaId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
