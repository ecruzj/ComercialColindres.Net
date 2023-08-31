using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class ConductoreMap : EntityTypeConfiguration<Conductores>
    {
        public ConductoreMap()
        {
            // Primary Key
            this.HasKey(t => t.ConductorId);

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Telefonos)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Conductores");
            this.Property(t => t.ConductorId).HasColumnName("ConductorId");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.Telefonos).HasColumnName("Telefonos");

            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.Conductores)
                .HasForeignKey(d => d.ProveedorId);

        }
    }
}
