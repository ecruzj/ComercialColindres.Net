using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletasImgMap : EntityConfiguration<BoletaImg>
    {
        public BoletasImgMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaId);

            // Properties
            this.Property(t => t.Imagen)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BoletasImg");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.Imagen).HasColumnName("Imagen");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithOptional(t => t.BoletaImg);
        }
    }
}
