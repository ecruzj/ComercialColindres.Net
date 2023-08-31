using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OrdenesCombustibleMap : EntityConfiguration<OrdenesCombustible>
    {
        public OrdenesCombustibleMap()
        {
            // Primary Key
            this.HasKey(t => t.OrdenCombustibleId);

            // Properties
            this.Property(t => t.CodigoFactura)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AutorizadoPor)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.TipoTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModificadoPor)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.TransaccionUId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("OrdenesCombustible");
            this.Property(t => t.OrdenCombustibleId).HasColumnName("OrdenCombustibleId");
            this.Property(t => t.CodigoFactura).HasColumnName("CodigoFactura");
            this.Property(t => t.GasCreditoId).HasColumnName("GasCreditoId");
            this.Property(t => t.AutorizadoPor).HasColumnName("AutorizadoPor");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.PlacaEquipo).HasColumnName("PlacaEquipo");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.EsOrdenPersonal).HasColumnName("EsOrdenPersonal");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasOptional(t => t.Boleta)
                .WithMany(t => t.OrdenesCombustible)
                .HasForeignKey(d => d.BoletaId);
            this.HasOptional(t => t.Proveedor)
                .WithMany(t => t.OrdenesCombustible)
                .HasForeignKey(d => d.ProveedorId);
            this.HasRequired(t => t.GasolineraCredito)
                .WithMany(t => t.OrdenesCombustibles)
                .HasForeignKey(d => d.GasCreditoId);
        }
    }
}
