using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CuentasFinancierasMap : EntityConfiguration<CuentasFinancieras>
    {
        public CuentasFinancierasMap()
        {
            // Primary Key
            this.HasKey(t => t.CuentaFinancieraId);

            // Properties
            this.Property(t => t.CuentaNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NombreAbonado)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Cedula)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ModificadoPor)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CuentasFinancieras");
            this.Property(t => t.CuentaFinancieraId).HasColumnName("CuentaFinancieraId");
            this.Property(t => t.BancoId).HasColumnName("BancoId");
            this.Property(t => t.CuentaFinancieraTipoId).HasColumnName("CuentaFinancieraTipoId");
            this.Property(t => t.CuentaNo).HasColumnName("CuentaNo");
            this.Property(t => t.NombreAbonado).HasColumnName("NombreAbonado");
            this.Property(t => t.Cedula).HasColumnName("Cedula");
            this.Property(t => t.EsCuentaAdministrativa).HasColumnName("EsCuentaAdministrativa");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");

            //Ignore
            this.Ignore(t => t.TipoTransaccion);
            this.Ignore(t => t.TransaccionUId);

            // Relationships
            this.HasRequired(t => t.Banco)
                .WithMany(t => t.CuentasFinancieras)
                .HasForeignKey(d => d.BancoId);
            this.HasRequired(t => t.CuentasFinancieraTipos)
                .WithMany(t => t.CuentasFinancieras)
                .HasForeignKey(d => d.CuentaFinancieraTipoId);
        }
    }
}
