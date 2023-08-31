using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class PrecioDescargasMap : EntityConfiguration<PrecioDescargas>
    {
        public PrecioDescargasMap()
        {
            // Primary Key
            this.HasKey(t => t.PrecioDescargaId);

            // Properties
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
            this.ToTable("PrecioDescargas");
            this.Property(t => t.PrecioDescargaId).HasColumnName("PrecioDescargaId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.EquipoCategoriaId).HasColumnName("EquipoCategoriaId");
            this.Property(t => t.PrecioDescarga).HasColumnName("PrecioDescarga");
            this.Property(t => t.EsPrecioActual).HasColumnName("EsPrecioActual");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");
            
            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.PrecioDescargas)
                .HasForeignKey(d => d.PlantaId);
            this.HasRequired(t => t.EquiposCategoria)
                .WithMany(t => t.PrecioDescargas)
                .HasForeignKey(d => d.EquipoCategoriaId);            
        }
    }
}
