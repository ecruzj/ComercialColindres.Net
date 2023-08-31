using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class SucursalesMap : EntityTypeConfiguration<Sucursales>
    {
        public SucursalesMap()
        {
            // Primary Key
            this.HasKey(t => t.SucursalId);

            // Properties
            this.Property(t => t.CodigoSucursal)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Direccion)
                .IsRequired();

            this.Property(t => t.Telefonos)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Sucursales");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.CodigoSucursal).HasColumnName("CodigoSucursal");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Direccion).HasColumnName("Direccion");
            this.Property(t => t.Telefonos).HasColumnName("Telefonos");
            this.Property(t => t.Estado).HasColumnName("Estado");
        }
    }
}
