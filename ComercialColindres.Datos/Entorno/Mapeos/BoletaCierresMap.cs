using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaCierresMap : EntityConfiguration<BoletaCierres>
    {
        public BoletaCierresMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaCierreId);

            // Properties
            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoDocumento)
                .HasMaxLength(50);

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
            this.ToTable("BoletaCierres");
            this.Property(t => t.BoletaCierreId).HasColumnName("BoletaCierreId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.LineaCreditoId).HasColumnName("LineaCreditoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.FechaPago).HasColumnName("FechaPago");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletaCierres)
                .HasForeignKey(d => d.BoletaId);
            this.HasRequired(t => t.LineasCredito)
                .WithMany(t => t.BoletaCierres)
                .HasForeignKey(d => d.LineaCreditoId);
        }
    }
}
