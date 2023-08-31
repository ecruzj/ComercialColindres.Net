using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class GasolineraCreditosMap : EntityConfiguration<GasolineraCreditos>
    {
        public GasolineraCreditosMap()
        {
            // Primary Key
            this.HasKey(t => t.GasCreditoId);

            // Properties
            this.Property(t => t.CodigoGasCredito)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CreadoPor)
                .IsRequired()
                .HasMaxLength(15);

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
            this.ToTable("GasolineraCreditos");
            this.Property(t => t.GasCreditoId).HasColumnName("GasCreditoId");
            this.Property(t => t.CodigoGasCredito).HasColumnName("CodigoGasCredito");
            this.Property(t => t.GasolineraId).HasColumnName("GasolineraId");
            this.Property(t => t.Credito).HasColumnName("Credito");
            this.Property(t => t.Saldo).HasColumnName("Saldo");
            this.Property(t => t.FechaInicio).HasColumnName("FechaInicio");
            this.Property(t => t.FechaFinal).HasColumnName("FechaFinal");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.CreadoPor).HasColumnName("CreadoPor");
            this.Property(t => t.EsCreditoActual).HasColumnName("EsCreditoActual");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Gasolinera)
                .WithMany(t => t.GasolineraCreditos)
                .HasForeignKey(d => d.GasolineraId);
        }
    }
}
