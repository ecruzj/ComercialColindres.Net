using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PagoPrestamosMap : EntityConfiguration<PagoPrestamos>
    {
        public PagoPrestamosMap()
        {
            // Primary Key
            this.HasKey(t => t.PagoPrestamoId);

            // Properties
            // Properties
            this.Property(t => t.FormaDePago)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoDocumento)
                .HasMaxLength(50);

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
            this.ToTable("PagoPrestamos");
            this.Property(t => t.PagoPrestamoId).HasColumnName("PagoPrestamoId");
            this.Property(t => t.PrestamoId).HasColumnName("PrestamoId");
            this.Property(t => t.FormaDePago).HasColumnName("FormaDePago");
            this.Property(t => t.BancoId).HasColumnName("BancoId");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.MontoAbono).HasColumnName("MontoAbono");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasOptional(t => t.Banco)
                .WithMany(t => t.PagoPrestamos)
                .HasForeignKey(d => d.BancoId);
            this.HasOptional(t => t.Boleta)
                .WithMany(t => t.PagoPrestamos)
                .HasForeignKey(d => d.BoletaId);
            this.HasRequired(t => t.Prestamo)
                .WithMany(t => t.PagoPrestamos)
                .HasForeignKey(d => d.PrestamoId);
        }
    }
}
