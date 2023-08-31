using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class CuentasFinancieraTiposMap : EntityConfiguration<CuentasFinancieraTipos>
    {
        public CuentasFinancieraTiposMap()
        {
            // Primary Key
            this.HasKey(t => t.CuentaFinancieraTipoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModificadoPor)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
                .HasMaxLength(50);
            
            // Table & Column Mappings
            this.ToTable("CuentasFinancieraTipos");
            this.Property(t => t.CuentaFinancieraTipoId).HasColumnName("CuentaFinancieraTipoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.RequiereBanco).HasColumnName("RequiereBanco");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");

            //Ignore
            this.Ignore(t => t.TipoTransaccion);
            this.Ignore(t => t.TransaccionUId);
        }
    }
}
