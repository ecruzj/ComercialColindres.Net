using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice
{
    public class RptPendingInvoiceDto : BaseDTO
    {
        public string NumeroFactura { get; set; }
        public string Planta { get; set; }
        public string Moneda { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaEmision { get; set; }
        public int Antiguedad { get; set; }
    }
}
