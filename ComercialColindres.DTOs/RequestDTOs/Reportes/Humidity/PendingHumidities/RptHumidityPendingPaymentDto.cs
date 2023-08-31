using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities
{
    public class RptHumidityPendingPaymentDto
    {
        public string Planta { get; set; }
        public string Estado { get; set; }
        public string NumeroEnvio { get; set; }
        public string Proveedor { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal Tolerancia { get; set; }
        public decimal Toneladas { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaHumedad { get; set; }
    }
}
