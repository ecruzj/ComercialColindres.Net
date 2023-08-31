using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class RecibosMap : EntityConfiguration<Recibos>
    {
        public RecibosMap()
        {
            // Primary Key
            this.HasKey(t => t.ReciboId);

            // Properties
            this.Property(t => t.AplicaA)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.NumeroRecibo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Observaciones)
                .IsRequired();

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Recibos");
            this.Property(t => t.ReciboId).HasColumnName("ReciboId");
            this.Property(t => t.SucursalId).HasColumnName("SucursalId");
            this.Property(t => t.EsAnticipo).HasColumnName("EsAnticipo");
            this.Property(t => t.AplicaA).HasColumnName("AplicaA");
            this.Property(t => t.PrestamoId).HasColumnName("PrestamoId");
            this.Property(t => t.FacturaId).HasColumnName("FacturaId");
            this.Property(t => t.NumeroRecibo).HasColumnName("NumeroRecibo");
            this.Property(t => t.Fecha).HasColumnName("Fecha");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");

            //Ignore
            this.Ignore(t => t.TipoTransaccion);
            this.Ignore(t => t.TransaccionUId);

            // Relationships
            this.HasOptional(t => t.Factura)
                .WithMany(t => t.Recibos)
                .HasForeignKey(d => d.FacturaId);
            this.HasOptional(t => t.Prestamo)
                .WithMany(t => t.Recibos)
                .HasForeignKey(d => d.PrestamoId);
            this.HasRequired(t => t.Sucursal)
                .WithMany(t => t.Recibos)
                .HasForeignKey(d => d.SucursalId);

        }
    }
}
