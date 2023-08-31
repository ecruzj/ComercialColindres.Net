using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class ConfiguracionesDetallesMap : EntityTypeConfiguration<ConfiguracionesDetalles>
    {
        public ConfiguracionesDetallesMap()
        {
            // Primary Key
            this.HasKey(t => t.ConfiguracionDetalleId);

            // Properties
            this.Property(t => t.CodigoConfiguracion)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Atributo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Valor)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ConfiguracionesDetalle");
            this.Property(t => t.ConfiguracionDetalleId).HasColumnName("ConfiguracionDetalleId");
            this.Property(t => t.CodigoConfiguracion).HasColumnName("CodigoConfiguracion");
            this.Property(t => t.Atributo).HasColumnName("Atributo");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.EsRequerido).HasColumnName("EsRequerido");

            // Relationships
            this.HasRequired(t => t.Configuracion)
                .WithMany(t => t.ConfiguracionesDetalles)
                .HasForeignKey(d => d.CodigoConfiguracion);

        }
    }
}
