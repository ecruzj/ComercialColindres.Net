using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FuelOrderManualPaymentMap : EntityConfiguration<FuelOrderManualPayment>
    {
        public FuelOrderManualPaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.FuelOrderManualPaymentId);

            // Properties
            this.Property(t => t.FuelOrderId)
                .IsRequired();

            this.Property(t => t.WayToPay)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankId);

            this.Property(t => t.PaymentDate)
                .IsRequired();

            this.Property(t => t.BankReference)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Amount)
                .IsRequired();

            this.Property(t => t.Observations)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FuelOrderManualPayment");
            this.Property(t => t.FuelOrderManualPaymentId).HasColumnName("FuelOrderManualPaymentId");
            this.Property(t => t.FuelOrderId).HasColumnName("FuelOrderId");
            this.Property(t => t.WayToPay).HasColumnName("WayToPay");
            this.Property(t => t.BankId).HasColumnName("BankId");
            this.Property(t => t.BankReference).HasColumnName("BankReference");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Observations).HasColumnName("Observations");

            // Relationships
            this.HasRequired(t => t.FuelOrder)
                .WithMany(t => t.FuelOrderManualPayments)
                .HasForeignKey(d => d.FuelOrderId);
            this.HasOptional(t => t.Bank)
                .WithMany(t => t.FuelOrderManualPayments)
                .HasForeignKey(d => d.BankId);
        }
    }
}
