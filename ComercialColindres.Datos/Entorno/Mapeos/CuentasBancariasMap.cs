using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CuentasBancariasMap : EntityTypeConfiguration<CuentasBancarias>
    {
        public CuentasBancariasMap()
        {
            // Primary Key
            this.HasKey(t => t.CuentaId);

            // Properties
            this.Property(t => t.BancoId)
                .IsRequired();

            this.Property(t => t.CuentaNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.NombreAbonado)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CedulaNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("CuentasBancarias");
            this.Property(t => t.CuentaId).HasColumnName("CuentaId");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.BancoId).HasColumnName("BancoId");
            this.Property(t => t.CuentaNo).HasColumnName("CuentaNo");
            this.Property(t => t.NombreAbonado).HasColumnName("NombreAbonado");
            this.Property(t => t.CedulaNo).HasColumnName("CedulaNo");
            this.Property(t => t.Estado).HasColumnName("Estado");

            // Relationships
            this.HasRequired(t => t.Banco)
                .WithMany(t => t.CuentasBancarias)
                .HasForeignKey(d => d.BancoId);
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.CuentasBancarias)
                .HasForeignKey(d => d.ProveedorId);

        }
    }
}
