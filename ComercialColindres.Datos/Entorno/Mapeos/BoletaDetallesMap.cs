using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaDetallesMap : EntityConfiguration<BoletaDetalles>
    {
        public BoletaDetallesMap()
        {
            // Primary Key
            this.HasKey(t => t.PagoBoletaId);

            // Properties
            this.Property(t => t.NoDocumento)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DescripcionTransaccion)
                .IsRequired()
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
            this.ToTable("BoletaDetalles");
            this.Property(t => t.PagoBoletaId).HasColumnName("PagoBoletaId");
            this.Property(t => t.CodigoBoleta).HasColumnName("CodigoBoleta");
            this.Property(t => t.DeduccionId).HasColumnName("DeduccionId");
            this.Property(t => t.MontoDeduccion).HasColumnName("MontoDeduccion");
            this.Property(t => t.NoDocumento).HasColumnName("NoDocumento");
            this.Property(t => t.Observaciones).HasColumnName("Observaciones");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletaDetalles)
                .HasForeignKey(d => d.CodigoBoleta);
            this.HasRequired(t => t.Deduccion)
                .WithMany(t => t.BoletaDetalles)
                .HasForeignKey(d => d.DeduccionId);

            this.Ignore(t => t.DescripcionDeduccion);
        }
    }
}
