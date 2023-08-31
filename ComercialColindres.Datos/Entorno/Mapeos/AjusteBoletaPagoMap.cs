using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class AjusteBoletaPagoMap : EntityConfiguration<AjusteBoletaPago>
    {
        public AjusteBoletaPagoMap()
        {
            // Primary Key
            this.HasKey(t => t.AjusteBoletaPagoId);

            // Properties
            this.Property(t => t.AjusteBoletaDetalleId)
                .IsRequired();

            this.Property(t => t.BoletaId)
                .IsRequired();
            
            // Table & Column Mappings
            this.ToTable("AjusteBoletaPagos");
            this.Property(t => t.AjusteBoletaPagoId).HasColumnName("AjusteBoletaPagoId");
            this.Property(t => t.AjusteBoletaDetalleId).HasColumnName("AjusteBoletaDetalleId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.FechaAbono).HasColumnName("FechaAbono");

            // Relationships
            this.HasRequired(t => t.AjusteBoletaDetalle)
                .WithMany(t => t.AjusteBoletaPagos)
                .HasForeignKey(d => d.AjusteBoletaDetalleId);
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.AjusteBoletaPagos)
                .HasForeignKey(d => d.BoletaId);
        }
    }
}
