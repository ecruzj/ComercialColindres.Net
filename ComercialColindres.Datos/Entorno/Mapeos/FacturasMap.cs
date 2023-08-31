using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class FacturasMap : EntityTypeConfiguration<Factura>
    {
        public FacturasMap()
        {
            this.HasKey(t => t.FacturaId);

            // Properties
            this.Property(t => t.TipoFactura)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.NumeroFactura)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ExonerationNo)
                .HasMaxLength(50);

            this.Property(t => t.OrdenCompra)
                .HasMaxLength(50);

            this.Property(t => t.Semana)
                .HasMaxLength(10);

            this.Property(t => t.Observaciones)
                .IsRequired();

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);
            
            // Table & Column Mappings
            this.ToTable("Facturas");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.FacturaCategoriaId).HasColumnName("FacturaCategoriaId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.SubPlantaId).HasColumnName("SubPlantaId");
            this.Property(t => t.OrdenCompra).HasColumnName("OrdenCompra");
            this.Property(t => t.Semana).HasColumnName("Semana");
            this.Property(t => t.TipoFactura).HasColumnName("TipoFactura");
            this.Property(t => t.NumeroFactura).HasColumnName("NumeroFactura");
            this.Property(t => t.ProFormaNo).HasColumnName("ProFormaNo");
            this.Property(t => t.Fecha).HasColumnName("Fecha");
            this.Property(t => t.IsExonerated).HasColumnName("IsExonerated");
            this.Property(t => t.ExonerationNo).HasColumnName("ExonerationNo");            
            this.Property(t => t.TaxPercent).HasColumnName("TaxPercent");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.IsForeignCurrency).HasColumnName("IsForeignCurrency");
            this.Property(t => t.LocalCurrencyAmount).HasColumnName("LocalCurrencyAmount");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.HasUnitPriceItem).HasColumnName("HasUnitPriceItem");

            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.Facturas)
                .HasForeignKey(d => d.PlantaId);
            this.HasRequired(t => t.FacturasCategoria)
                .WithMany(t => t.Facturas)
                .HasForeignKey(d => d.FacturaCategoriaId);
            this.HasRequired(t => t.Sucursal)
                .WithMany(t => t.Facturas)
                .HasForeignKey(d => d.SucursalId);
            this.HasOptional(t => t.SubFacility)
                .WithMany(t => t.Facturas)
                .HasForeignKey(d => d.SubPlantaId);
        }
    }
}
