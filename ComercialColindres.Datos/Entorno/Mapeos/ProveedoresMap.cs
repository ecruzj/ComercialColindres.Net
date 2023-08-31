using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class ProveedoresMap : EntityTypeConfiguration<Proveedores>
    {
        public ProveedoresMap()
        {
            // Primary Key
            this.HasKey(t => t.ProveedorId);

            // Properties
            this.Property(t => t.CedulaNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.RTN)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Direccion)
                .IsRequired();

            this.Property(t => t.Telefonos)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CorreoElectronico)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Proveedores");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.CedulaNo).HasColumnName("CedulaNo");
            this.Property(t => t.RTN).HasColumnName("RTN");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Direccion).HasColumnName("Direccion");
            this.Property(t => t.Telefonos).HasColumnName("Telefonos");
            this.Property(t => t.CorreoElectronico).HasColumnName("CorreoElectronico");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.IsExempt).HasColumnName("IsExempt");
        }
    }
}
