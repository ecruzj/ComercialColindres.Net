using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PagoOrdenesCombustibleMap : EntityConfiguration<PagoOrdenesCombustible>
    {

        public PagoOrdenesCombustibleMap()
        {
            // Primary Key
            this.HasKey(t => t.PagoOrdenCombustibleId);

            // Properties
            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoDocumento)
                .HasMaxLength(50);

            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.TipoTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModificadoPor)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.TransaccionUId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PagoOrdenesCombustible");
            this.Property(t => t.PagoOrdenCombustibleId).HasColumnName("PagoOrdenCombustibleId");
            this.Property(t => t.OrdenCombustibleId).HasColumnName("OrdenCombustibleId");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.MontoAbono).HasColumnName("MontoAbono");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.LineaCredito)
                .WithMany(t => t.PagoOrdenesCombustibles)
                .HasForeignKey(d => d.LineaCreditoId);
            this.HasOptional(t => t.Boleta)
                .WithMany(t => t.PagoOrdenesCombustible)
                .HasForeignKey(d => d.BoletaId);

        }
    }
}
