using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PrestamosTransferenciasMap : EntityConfiguration<PrestamosTransferencias>
    {
        public PrestamosTransferenciasMap()
        {
            // Primary Key
            this.HasKey(t => t.PrestamoTransferenciaId);

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
            this.ToTable("PrestamoTransferencias");
            this.Property(t => t.PrestamoTransferenciaId).HasColumnName("PrestamoTransferenciaId");
            this.Property(t => t.PrestamoId).HasColumnName("PrestamoId");
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
                .WithMany(t => t.PrestamoTransferencias)
                .HasForeignKey(d => d.LineaCreditoId);
            this.HasRequired(t => t.Prestamo)
                .WithMany(t => t.PrestamosTransferencias)
                .HasForeignKey(d => d.PrestamoId);
        }
    }
}
