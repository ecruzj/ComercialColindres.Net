using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class ReporteBoletaDeduccionesPrestamoDTO
    {
        public string CodigoPrestamo { get; set; }
        public decimal MontoPrestamo { get; set; }
        public decimal PorcentajeInteres { get; set; }
        public string AutorizadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CodigoBoleta { get; set; }
        public string NombrePlanta { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Motorista { get; set; }
        public string PlacaEquipo { get; set; }
        public string TipoProducto { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal TasaSeguridad { get; set; }
        public string CodigoFactura { get; set; }
        public decimal MontoCombustible { get; set; }
        public decimal MontoAbono { get; set; }
        public string Cuadrilla { get; set; }
        public decimal PrecioDescarga { get; set; }
        public string MotivoDeduccion { get; set; }
        public decimal MontoDeduccion { get; set; }

        public string DescripcionDeduccion { get; set; }
        public string ObservacionesDeduccion { get; set; }
    }
}
