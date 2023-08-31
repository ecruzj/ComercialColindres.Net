using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FacturaPagoMap : EntityConfiguration<FacturaPago>
    {
        public FacturaPagoMap()
        {
            // Primary Key
            this.HasKey(t => t.FacturaPagoId);

            // Properties
            this.Property(t => t.FacturaId)
                .IsRequired();

            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BancoId)
                .IsRequired();

            this.Property(t => t.ReferenciaBancaria)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FacturaPagos");
            this.Property(t => t.FacturaPagoId).HasColumnName("FacturaPagoId");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.BancoId).HasColumnName("BancoId");
            this.Property(t => t.FechaDePago).HasColumnName("FechaDePago");
            this.Property(t => t.ReferenciaBancaria).HasColumnName("ReferenciaBancaria");
            this.Property(t => t.Monto).HasColumnName("Monto");

            // Relationships
            this.HasRequired(t => t.Invoice)
                .WithMany(t => t.FacturaPagos)
                .HasForeignKey(d => d.FacturaId);
            this.HasRequired(t => t.Bank)
                .WithMany(t => t.FacturaPagos)
                .HasForeignKey(d => d.BancoId);
        }
    }
}
