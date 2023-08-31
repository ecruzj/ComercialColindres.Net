using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptPrestamosPorProveedorEncabezadoDTO : BaseDTO
    {
        public string CodigoPrestamo { get; set; }
        public string AutorizadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal PorcentajeInteres { get; set; }
        public string EsInteresMensual { get; set; }
        public decimal Intereses { get; set; }
        public decimal MontoPrestamo { get; set; }
        public decimal TotalCobrar { get; set; }
        public decimal TotalAbono { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Estado { get; set; }
        public string Proveedor { get; set; }
        public string Observaciones { get; set; }
    }
}
