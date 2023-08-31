using ComercialColindres.Datos.Entorno.Entidades;
using System.Data.Entity.ModelConfiguration;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class ClientePlantasMap : EntityTypeConfiguration<ClientePlantas>
    {
        public ClientePlantasMap()
        {
            // Primary Key
            this.HasKey(t => t.PlantaId);

            // Properties
            this.Property(t => t.RTN)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.NombrePlanta)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Telefonos)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Direccion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ClientePlantas");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.RTN).HasColumnName("RTN");
            this.Property(t => t.NombrePlanta).HasColumnName("NombrePlanta");
            this.Property(t => t.Telefonos).HasColumnName("Telefonos");
            this.Property(t => t.Direccion).HasColumnName("Direccion");
            this.Property(t => t.RequiresPurchaseOrder).HasColumnName("RequiresPurchaseOrder");
            this.Property(t => t.RequiresWeekNo).HasColumnName("RequiresWeekNo");
            this.Property(t => t.RequiresProForm).HasColumnName("RequiresProForm");
            this.Property(t => t.IsExempt).HasColumnName("IsExempt");
            this.Property(t => t.HasSubPlanta).HasColumnName("HasSubPlanta");
            this.Property(t => t.HasExonerationNo).HasColumnName("HasExonerationNo");
            this.Property(t => t.HasExonerationNo).HasColumnName("HasExonerationNo");
            this.Property(t => t.ImgHorizontalFormat).HasColumnName("ImgHorizontalFormat");
        }
    }
}
