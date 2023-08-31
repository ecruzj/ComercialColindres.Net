using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError
{
    public class RptBillWithWeightsErrorDto
    {
        public string Planta { get; set; }
        public int FacturaId { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Biomasa { get; set; }
        public decimal VentaTone { get; set; }
        public decimal CompraTone { get; set; }
        public string Moneda { get; set; }
        public decimal PrecioTone { get; set; }
        public decimal DiferenciaTone { get; set; }
        public decimal Saldo { get; set; }
        public bool MustEvaluate { get; set; }
    }
}
