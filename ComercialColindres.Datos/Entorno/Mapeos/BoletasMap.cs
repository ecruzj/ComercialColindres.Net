using ComercialColindres.Datos.Entorno.Context;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Datos.Entorno.Mapeos
{
    public class BoletasMap : EntityConfiguration<Boletas>
    {
        public BoletasMap()
        {
            // Primary Key
            this.HasKey(t => t.BoletaId);

            // Properties
            this.Property(t => t.CodigoBoleta)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.NumeroEnvio)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.PlacaEquipo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Motorista)
                .IsRequired()
                .HasMaxLength(50);

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

            this.Property(t => t.TransaccionUId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Boletas");
            this.Property(t => t.BoletaId).HasColumnName("BoletaId");
            this.Property(t => t.CodigoBoleta).HasColumnName("CodigoBoleta");
            this.Property(t => t.NumeroEnvio).HasColumnName("NumeroEnvio");
            this.Property(t => t.ProveedorId).HasColumnName("ProveedorId");
            this.Property(t => t.PlacaEquipo).HasColumnName("PlacaEquipo");
            this.Property(t => t.Motorista).HasColumnName("Motorista");
            this.Property(t => t.CategoriaProductoId).HasColumnName("CategoriaProductoId");
            this.Property(t => t.PlantaId).HasColumnName("PlantaId");
            this.Property(t => t.PesoEntrada).HasColumnName("PesoEntrada");
            this.Property(t => t.PesoSalida).HasColumnName("PesoSalida");            
            this.Property(t => t.PesoProducto).HasColumnName("PesoProducto");
            this.Property(t => t.CantidadPenalizada).HasColumnName("CantidadPenalizada");
            this.Property(t => t.Bonus).HasColumnName("Bonus");
            this.Property(t => t.PrecioProductoCompra).HasColumnName("PrecioProductoCompra");
            this.Property(t => t.PrecioProductoVenta).HasColumnName("PrecioProductoVenta");
            this.Property(t => t.FechaSalida).HasColumnName("FechaSalida");
            this.Property(t => t.FechaCreacionBoleta).HasColumnName("FechaCreacionBoleta");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.BoletaPath).HasColumnName("BoletaPath");
            this.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion");
            this.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion");
            this.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor");
            this.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            this.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");

            // Relationships
            this.HasRequired(t => t.CategoriaProducto)
                .WithMany(t => t.Boletas)
                .HasForeignKey(d => d.CategoriaProductoId);          
            this.HasRequired(t => t.ClientePlanta)
                .WithMany(t => t.Boletas)
                .HasForeignKey(d => d.PlantaId);
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.Boletas)
                .HasForeignKey(d => d.ProveedorId);
        }
    }
}
