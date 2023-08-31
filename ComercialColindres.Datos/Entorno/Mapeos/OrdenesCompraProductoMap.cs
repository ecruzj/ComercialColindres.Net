using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class OrdenesCompraProductoMap : EntityConfiguration<OrdenesCompraProducto>
    {
        public OrdenesCompraProductoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrdenCompraProductoId);

            // Properties
            this.Property(t => t.OrdenCompraNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoExoneracionDEI)
                .IsRequired()
                .HasMaxLength(50);

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
            this.ToTable("OrdenesCompraProducto");
            this.Property(t => t.OrdenCompraProductoId).HasColumnName("OrdenCompraProductoId");
            this.Property(t => t.OrdenCompraNo).HasColumnName("OrdenCompraNo");
            this.Property(t => t.NoExoneracionDEI).HasColumnName("NoExoneracionDEI");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.FechaCreacion).HasColumnName("FechaCreacion");
            this.Property(t => t.FechaActivacion).HasColumnName("FechaActivacion");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.FechaCierre).HasColumnName("FechaCierre");
            this.Property(t => t.CreadoPor).HasColumnName("CreadoPor");
            this.Property(t => t.MontoDollar).HasColumnName("MontoDollar");
            this.Property(t => t.ConversionDollarToLps).HasColumnName("ConversionDollarToLps").HasPrecision(18, 10);
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.OrdenesCompraProductos)
                .HasForeignKey(d => d.PlantaId);

        }
    }
}
