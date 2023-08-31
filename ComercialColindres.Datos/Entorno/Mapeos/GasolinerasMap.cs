using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class GasolinerasMap : EntityTypeConfiguration<Gasolineras>
    {
        public GasolinerasMap()
        {
            // Primary Key
            this.HasKey(t => t.GasolineraId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NombreContacto)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TelefonoContacto)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Gasolineras");
            this.Property(t => t.GasolineraId).HasColumnName("GasolineraId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.NombreContacto).HasColumnName("NombreContacto");
            this.Property(t => t.TelefonoContacto).HasColumnName("TelefonoContacto");
        }
    }
}
