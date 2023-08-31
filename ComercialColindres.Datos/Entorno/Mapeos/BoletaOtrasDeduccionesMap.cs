using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaOtrasDeduccionesMap : EntityConfiguration<BoletaOtrasDeducciones>
    {
        public BoletaOtrasDeduccionesMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaOtraDeduccionId);

            // Properties
            this.Property(t => t.MotivoDeduccion)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoDocumento)
                .IsRequired()
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
            this.ToTable("BoletaOtrasDeducciones");
            this.Property(t => t.BoletaOtraDeduccionId).HasColumnName("BoletaOtraDeduccionId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.MotivoDeduccion).HasColumnName("MotivoDeduccion");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletaOtrasDeducciones)
                .HasForeignKey(d => d.BoletaId);
            this.HasOptional(t => t.LineasCredito)
                .WithMany(t => t.BoletaOtrasDeducciones)
                .HasForeignKey(d => d.LineaCreditoId);

        }
    }
}
