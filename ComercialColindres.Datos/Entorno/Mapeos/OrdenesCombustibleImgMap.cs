using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OrdenesCombustibleImgMap : EntityConfiguration<OrdenCombustibleImg>
    {
        public OrdenesCombustibleImgMap()
        {
            // Primary Key
            this.HasKey(t => t.OrdenCombustibleId);

            // Properties
            this.Property(t => t.Imagen)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("OrdenesCombustibleImg");
            this.Property(t => t.OrdenCombustibleId).HasColumnName("OrdenCombustibleId");
            this.Property(t => t.Imagen).HasColumnName("Imagen");

            // Relationships
            this.HasRequired(t => t.OrdenCombustible)
                .WithOptional(t => t.OrdenCombustibleImg);
        }
    }
}
