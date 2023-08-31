using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class SubPlantaMap : EntityTypeConfiguration<SubPlanta>
    {
        public SubPlantaMap()
        {
            // Primary Key
            this.HasKey(t => t.SubPlantaId);

            //Properties
            this.Property(t => t.NombreSubPlanta)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Rtn)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Direccion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SubPlantas");
            this.Property(t => t.SubPlantaId).HasColumnName("SubPlantaId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.NombreSubPlanta).HasColumnName("NombreSubPlanta");
            this.Property(t => t.Rtn).HasColumnName("Rtn");
            this.Property(t => t.Direccion).HasColumnName("Direccion");
            this.Property(t => t.EsExonerado).HasColumnName("EsExonerado");
            this.Property(t => t.RegistroExoneracion).HasColumnName("RegistroExoneracion");

            // Relationships
            this.HasRequired(t => t.Planta)
                .WithMany(t => t.SubPlantas)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
