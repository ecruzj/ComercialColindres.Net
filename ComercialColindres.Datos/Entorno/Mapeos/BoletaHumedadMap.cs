using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaHumedadMap : EntityConfiguration<BoletaHumedad>
    {
        public BoletaHumedadMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaHumedadId);

            // Properties
            this.Property(t => t.NumeroEnvio)
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
            this.ToTable("BoletasHumedad");
            this.Property(t => t.BoletaHumedadId).HasColumnName("BoletaHumedadId");
            this.Property(t => t.NumeroEnvio).HasColumnName("NumeroEnvio");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.BoletaIngresada).HasColumnName("BoletaIngresada");
            this.Property(t => t.HumedadPromedio).HasColumnName("HumedadPromedio");
            this.Property(t => t.PorcentajeTolerancia).HasColumnName("PorcentajeTolerancia");
            this.Property(t => t.FechaHumedad).HasColumnName("FechaHumedad");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.BoletasHumedad)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
