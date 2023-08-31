using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OpcionesMap : EntityTypeConfiguration<Opciones>
    {
        public OpcionesMap()
        {
            // Primary Key
            this.HasKey(t => t.OpcionId);

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TipoAcceso)
               .IsRequired()
               .HasMaxLength(20);

            this.Property(t => t.TipoPropiedad)
               .IsRequired()
               .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Opciones");
            this.Property(t => t.OpcionId).HasColumnName("OpcionId");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.TipoAcceso).HasColumnName("TipoAcceso");
            this.Property(t => t.TipoPropiedad).HasColumnName("TipoPropiedad");
        }
    }
}
