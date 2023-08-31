using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaHumedadPagoMap : EntityConfiguration<BoletaHumedadPago>
    {
        public BoletaHumedadPagoMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaHumedadId);

            // Properties
            this.Property(t => t.BoletaHumedadPagoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.BoletaHumedadId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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
            this.ToTable("BoletasHumedadPago");
            this.Property(t => t.BoletaHumedadPagoId).HasColumnName("BoletaHumedadPagoId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.BoletaHumedadId).HasColumnName("BoletaHumedadId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletasHumedadPagos)
                .HasForeignKey(d => d.BoletaId);
            this.HasRequired(t => t.BoletaHumedad)
                .WithOptional(t => t.BoletaHumedadPago);

        }
    }
}
