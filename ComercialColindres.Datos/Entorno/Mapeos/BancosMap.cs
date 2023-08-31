using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BancosMap : EntityTypeConfiguration<Bancos>
    {
        public BancosMap()
        {
            // Primary Key
            this.HasKey(t => t.BancoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Bancos");
            this.Property(t => t.BancoId).HasColumnName("BancoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
