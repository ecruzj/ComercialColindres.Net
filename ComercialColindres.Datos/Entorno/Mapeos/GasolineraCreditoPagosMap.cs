using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class GasolineraCreditoPagosMap : EntityConfiguration<GasolineraCreditoPagos>
    {
        public GasolineraCreditoPagosMap()
        {
            // Primary Key
            this.HasKey(t => t.GasCreditoPagoId);

            // Properties
            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoDocumento)
                .HasMaxLength(50);

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
            this.ToTable("GasolineraCreditoPagos");
            this.Property(t => t.GasCreditoPagoId).HasColumnName("GasCreditoPagoId");
            this.Property(t => t.GasCreditoId).HasColumnName("GasCreditoId");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.GasolineraCredito)
                .WithMany(t => t.GasolineraCreditoPagos)
                .HasForeignKey(d => d.GasCreditoId);
            this.HasRequired(t => t.LineaCredito)
                .WithMany(t => t.GasolineraCreditoPagos)
                .HasForeignKey(d => d.LineaCreditoId);
        }
    }
}
