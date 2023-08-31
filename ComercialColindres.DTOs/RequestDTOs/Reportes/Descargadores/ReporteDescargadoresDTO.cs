using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class ReporteDescargadoresDTO
    {
        public string CodigoPagoDescarga { get; set; }
        public DateTime FechaPago { get; set; }
        public string Cuadrilla { get; set; }
        public string Planta { get; set; }
        public string Estado { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Proveedor { get; set; }
        public string PlacaEquipo { get; set; }
        public string Motorista { get; set; }
        public string TipoProducto { get; set; }
        public string TipoEquipo { get; set; }
        public decimal PrecioDescarga { get; set; }
    }
}
