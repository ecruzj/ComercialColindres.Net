using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class LineasCreditoMap : EntityConfiguration<LineasCredito>
    {
        public LineasCreditoMap()
        {
            // Primary Key
            this.HasKey(t => t.LineaCreditoId);

            // Properties
            this.Property(t => t.CodigoLineaCredito)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.NoDocumento)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreadoPor)
                .IsRequired()
                .HasMaxLength(10);

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
            this.ToTable("LineasCredito");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.CodigoLineaCredito).HasColumnName("CodigoLineaCredito");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.CuentaFinancieraId).HasColumnName("CuentaFinancieraId");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.FechaCierre).HasColumnName("FechaCierre");
            this.Property(t => t.MontoInicial).HasColumnName("MontoInicial");
            this.Property(t => t.Saldo).HasColumnName("Saldo");
            this.Property(t => t.CreadoPor).HasColumnName("CreadoPor");
            this.Property(t => t.EsLineaCreditoActual).HasColumnName("EsLineaCreditoActual");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.CuentasFinanciera)
                .WithMany(t => t.LineasCreditoes)
                .HasForeignKey(d => d.CuentaFinancieraId);

        }
    }
}
