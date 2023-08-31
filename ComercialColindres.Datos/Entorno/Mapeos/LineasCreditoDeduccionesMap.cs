using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class LineasCreditoDeduccionesMap : EntityConfiguration<LineasCreditoDeducciones>
    {
        public LineasCreditoDeduccionesMap()
        {
            // Primary Key
            this.HasKey(t => t.LineaCreditoDeduccionId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(200);

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
            this.ToTable("LineasCreditoDeducciones");
            this.Property(t => t.LineaCreditoDeduccionId).HasColumnName("LineaCreditoDeduccionId");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.EsGastoOperativo).HasColumnName("EsGastoOperativo");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.LineasCredito)
                .WithMany(t => t.LineasCreditoDeducciones)
                .HasForeignKey(d => d.LineaCreditoId);

            //Ignore
            this.Ignore(t => t.RequiereBanco);
        }
    }
}
