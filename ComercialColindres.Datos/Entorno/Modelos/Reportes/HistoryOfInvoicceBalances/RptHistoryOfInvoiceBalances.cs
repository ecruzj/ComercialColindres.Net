using System;

namespace ComercialColindres.Datos.Entorno.Modelos.Reportes.HistoryOfInvoicceBalances
{
    public class RptHistoryOfInvoiceBalances
    {
        public int FacturaId { get; set; }
        public string NumeroFactura { get; set; }
        public string OrdenCompra { get; set; }
        public string Semana { get; set; }
        public DateTime FechaFactura { get; set; }
        public string Observaciones { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal SaldoActual { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
