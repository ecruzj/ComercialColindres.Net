using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class EquiposMap : EntityTypeConfiguration<Equipos>
    {
        public EquiposMap()
        {
            // Primary Key
            this.HasKey(t => t.EquipoId);

            // Properties
            this.Property(t => t.PlacaCabezal)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Equipos");
            this.Property(t => t.EquipoId).HasColumnName("EquipoId");
            this.Property(t => t.EquipoCategoriaId).HasColumnName("EquipoCategoriaId");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.PlacaCabezal).HasColumnName("PlacaCabezal");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.Equipos)
                .HasForeignKey(d => d.ProveedorId);
            this.HasRequired(t => t.EquiposCategoria)
                .WithMany(t => t.Equipos)
                .HasForeignKey(d => d.EquipoCategoriaId);
        }
    }
}
