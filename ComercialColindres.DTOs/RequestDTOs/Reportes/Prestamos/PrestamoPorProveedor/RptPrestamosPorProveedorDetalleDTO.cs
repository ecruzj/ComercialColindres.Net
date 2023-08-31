using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptPrestamosPorProveedorDetalleDTO : BaseDTO
    {
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Estado { get; set; }
        public string NombrePlanta { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Motorista { get; set; }
        public string PlacaEquipo { get; set; }
        public string TipoProducto { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal TasaSeguridad { get; set; }
        public decimal MontoCombustible { get; set; }
        public decimal MontoAbono { get; set; }
        public decimal PagoCliente { get; set; }
        public decimal PrecioDescarga { get; set; }
        public decimal OtrasDeducciones { get; set; }
        public decimal OtrosIngresos { get; set; }
        public string CodigoPrestamo { get; set; }
    }
}
