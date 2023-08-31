using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class AjusteBoletaMap : EntityConfiguration<AjusteBoleta>
    {
        public AjusteBoletaMap()
        {
            // Primary Key
            this.HasKey(t => t.AjusteBoletaId);

            // Properties
            this.Property(t => t.BoletaId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AjusteBoletas");
            this.Property(t => t.AjusteBoletaId).HasColumnName("AjusteBoletaId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.AjusteBoletas)
                .HasForeignKey(d => d.BoletaId);
        }
    }
}
