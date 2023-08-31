using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class DescargasPorAdelantadoMap : EntityConfiguration<DescargasPorAdelantado>
    {
        public DescargasPorAdelantadoMap()
        {
            // Primary Key
            this.HasKey(t => t.DescargaPorAdelantadoId);

            // Properties
            this.Property(t => t.DescargaPorAdelantadoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.NumeroEnvio)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CodigoBoleta)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.PlantaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreadoPor)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Estado)
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

            // Table & Column Mappings
            this.ToTable("DescargasPorAdelantado");
            this.Property(t => t.DescargaPorAdelantadoId).HasColumnName("DescargaPorAdelantadoId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.NumeroEnvio).HasColumnName("NumeroEnvio");
            this.Property(t => t.CodigoBoleta).HasColumnName("CodigoBoleta"); 
            this.Property(t => t.PagoDescargaId).HasColumnName("PagoDescargaId");
            this.Property(t => t.CreadoPor).HasColumnName("CreadoPor");
            this.Property(t => t.PrecioDescarga).HasColumnName("PrecioDescarga");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.DescargasPorAdelantado)
                .HasForeignKey(d => d.PlantaId);
            this.HasRequired(t => t.PagoDescargador)
                .WithMany(t => t.DescargasPorAdelantado)
                .HasForeignKey(d => d.PagoDescargaId);
            this.HasOptional(t => t.Boleta)
                .WithMany(t => t.DescargasPorAdelantado)
                .HasForeignKey(d => d.BoletaId);

            //Ignore
            this.Ignore(t => t.HasPayment);
        }
    }
}
