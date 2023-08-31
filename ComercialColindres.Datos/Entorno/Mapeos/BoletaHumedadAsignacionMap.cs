using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaHumedadAsignacionMap : EntityConfiguration<BoletaHumedadAsignacion>
    {
        public BoletaHumedadAsignacionMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaHumedadId);

            // Properties
            this.Property(t => t.BoletaHumedadAsignacionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.BoletaHumedadId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("BoletasHumedadAsignacion");
            this.Property(t => t.BoletaHumedadAsignacionId).HasColumnName("BoletaHumedadAsignacionId");
            this.Property(t => t.BoletaHumedadId).HasColumnName("BoletaHumedadId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletasHumedadAsignacion)
                .HasForeignKey(t => t.BoletaId);
            this.HasRequired(t => t.BoletaHumedad)
                .WithOptional(t => t.BoletaHumedadAsignacion);

        }
    }
}
