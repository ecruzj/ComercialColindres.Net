using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PagoDescargadoresMap : EntityConfiguration<PagoDescargadores>
    {
        public PagoDescargadoresMap()
        {
            // Primary Key
            this.HasKey(t => t.PagoDescargaId);

            // Properties
            this.Property(t => t.CodigoPagoDescarga)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CreadoPor)
                .IsRequired()
                .HasMaxLength(10);

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
            this.ToTable("PagoDescargadores");
            this.Property(t => t.PagoDescargaId).HasColumnName("PagoDescargaId");
            this.Property(t => t.CodigoPagoDescarga).HasColumnName("CodigoPagoDescarga");
            this.Property(t => t.CuadrillaId).HasColumnName("CuadrillaId");
            this.Property(t => t.FechaPago).HasColumnName("FechaPago");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.CreadoPor).HasColumnName("CreadoPor");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Cuadrilla)
                .WithMany(t => t.PagoDescargadores)
                .HasForeignKey(d => d.CuadrillaId);

        }
    }
}
