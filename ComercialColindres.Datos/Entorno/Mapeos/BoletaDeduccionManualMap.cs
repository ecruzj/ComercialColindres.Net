using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletaDeduccionManualMap : EntityConfiguration<BoletaDeduccionManual>
    {
        public BoletaDeduccionManualMap()
        {
            // Primary Key
            this.HasKey(t => t.DeduccionManualId);

            // Properties
            this.Property(t => t.MotivoDeduccion)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BoletaDeduccionesManual");
            this.Property(t => t.DeduccionManualId).HasColumnName("DeduccionManualId");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.Monto).HasColumnName("Monto");
            this.Property(t => t.MotivoDeduccion).HasColumnName("MotivoDeduccion");
            
            // Relationships
            this.HasRequired(t => t.Boleta)
                .WithMany(t => t.BoletaDeduccionesManual)
                .HasForeignKey(d => d.BoletaId);

        }
    }
}
