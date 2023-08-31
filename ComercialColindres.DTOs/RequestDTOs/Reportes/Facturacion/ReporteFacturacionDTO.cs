using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class ReporteFacturacionDTO
    {
        public string Planta { get; set; }
        public string Empresa { get; set; }
        public string TipoFactura { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaFacturacion { get; set; }
        public decimal TotalFactura { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Proveedor { get; set; }
        public string PlacaEquipo { get; set; }
        public string Motorista { get; set; }
        public string TipoProducto { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal TotalBoleta { get; set; }
        public DateTime FechaSalida { get; set; }
    }
}
