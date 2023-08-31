using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class AjusteBoletaDetalleMap : EntityConfiguration<AjusteBoletaDetalle>
    {
        public AjusteBoletaDetalleMap()
        {
            // Primary Key
            this.HasKey(t => t.AjusteBoletaDetalleId);

            // Properties
            this.Property(t => t.AjusteBoletaId)
                .IsRequired();

            this.Property(t => t.AjusteTipoId)
                .IsRequired();

            this.Property(t => t.Monto)
                .IsRequired();
            
            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AjusteBoletaDetalles");
            this.Property(t => t.AjusteBoletaDetalleId).HasColumnName("AjusteBoletaDetalleId");
            this.Property(t => t.AjusteBoletaId).HasColumnName("AjusteBoletaId");
            this.Property(t => t.AjusteTipoId).HasColumnName("AjusteTipoId");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");

            // Relationships
            this.HasRequired(t => t.AjusteTipo)
                .WithMany(t => t.AjusteBoletaDetalles)
                .HasForeignKey(d => d.AjusteTipoId);
            this.HasRequired(t => t.AjusteBoleta)
                .WithMany(t => t.AjusteBoletaDetalles)
                .HasForeignKey(d => d.AjusteBoletaId);
            this.HasOptional(t => t.LineaCredito)
                .WithMany(t => t.AjusteBoletaDetalles)
                .HasForeignKey(d => d.LineaCreditoId);
        }
    }
}
