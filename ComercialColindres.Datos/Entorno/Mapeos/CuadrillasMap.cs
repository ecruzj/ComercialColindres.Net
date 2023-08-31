using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CuadrillasMap : EntityTypeConfiguration<Cuadrillas>
    {
        public CuadrillasMap()
        {
            // Primary Key
            this.HasKey(t => t.CuadrillaId);

            // Properties
            this.Property(t => t.NombreEncargado)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Cuadrillas");
            this.Property(t => t.CuadrillaId).HasColumnName("CuadrillaId");
            this.Property(t => t.NombreEncargado).HasColumnName("NombreEncargado");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.AplicaPago).HasColumnName("AplicaPago");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.Cuadrillas)
                .HasForeignKey(d => d.PlantaId);
        }
    }
}
