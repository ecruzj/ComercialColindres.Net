using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class NotaCreditoMap : EntityConfiguration<NotaCredito>
    {
        public NotaCreditoMap()
        {
            //Primary Key
            this.HasKey(t => t.NotaCreditoId);

            //Properties
            this.Property(t => t.FacturaId)
                .IsRequired();

            this.Property(t => t.NotaCreditoNo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(50);

            //Table & Colum Mappings
            this.ToTable("NotasCredito");
            this.Property(t => t.NotaCreditoId).HasColumnName("NotaCreditoId");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.NotaCreditoNo).HasColumnName("NotaCreditoNo");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");

            //Relationships
            this.HasRequired(t => t.Invoice)
                .WithMany(t => t.NotasCredito)
                .HasForeignKey(d => d.FacturaId);
        }
    }
}
