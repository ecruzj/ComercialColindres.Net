using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class DescargadoresMap : EntityConfiguration<Descargadores>
    {
        public DescargadoresMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaId);

            // Properties
            this.Property(t => t.DescargadaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.BoletaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Estado)
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

            // Table & Column Mappings
            this.ToTable("Descargadores");
            this.Property(t => t.DescargadaId).HasColumnName("DescargadaId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.CuadrillaId).HasColumnName("CuadrillaId");
            this.Property(t => t.PrecioDescarga).HasColumnName("PrecioDescarga");
            this.Property(t => t.PagoDescarga).HasColumnName("PagoDescarga");
            this.Property(t => t.PagoDescargaId).HasColumnName("PagoDescargaId");
            this.Property(t => t.EsDescargaPorAdelanto).HasColumnName("EsDescargaPorAdelanto");
            this.Property(t => t.EsIngresoManual).HasColumnName("EsIngresoManual");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.FechaDescarga).HasColumnName("FechaDescarga");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithOptional(t => t.Descargador);
            this.HasRequired(t => t.Cuadrilla)
                .WithMany(t => t.Descargadores)
                .HasForeignKey(d => d.CuadrillaId);
            this.HasOptional(t => t.PagoDescargador)
                .WithMany(t => t.Descargadores)
                .HasForeignKey(d => d.PagoDescargaId);
        }
    }
}
