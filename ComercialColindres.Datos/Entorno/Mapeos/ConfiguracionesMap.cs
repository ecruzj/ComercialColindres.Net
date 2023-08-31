using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class ConfiguracionesMap : EntityTypeConfiguration<Configuraciones>
    {
        public ConfiguracionesMap()
        {
            // Primary Key
            this.HasKey(t => t.CodigoConfiguracion);

            // Properties
            this.Property(t => t.CodigoConfiguracion)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Configuraciones");
            this.Property(t => t.CodigoConfiguracion).HasColumnName("CodigoConfiguracion");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.DefinidaPorUsuario).HasColumnName("DefinidaPorUsuario");
        }
    }
}
