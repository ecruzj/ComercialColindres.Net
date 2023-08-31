using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PagoDescargaDetalleMap : EntityConfiguration<PagoDescargaDetalles>
    {
        public PagoDescargaDetalleMap()
        {
            // Primary Key
            this.HasKey(t => t.PagoDescargaDetalleId);

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
            this.ToTable("PagoDescargaDetalles");
            this.Property(t => t.PagoDescargaDetalleId).HasColumnName("PagoDescargaDetalleId");
            this.Property(t => t.PagoDescargaId).HasColumnName("PagoDescargaId");
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
            this.HasRequired(t => t.LineaCredito)
                .WithMany(t => t.PagoDescargaDetalles)
                .HasForeignKey(d => d.LineaCreditoId);
            this.HasRequired(t => t.PagoDescargador)
                .WithMany(t => t.PagoDescargaDetalles)
                .HasForeignKey(d => d.PagoDescargaId);

        }
    }
}
