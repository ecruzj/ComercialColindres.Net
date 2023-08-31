using System;

namespace ComercialColindres.Datos.Entorno.Modelos.Reportes.PendingInvoice
{
    public class RptPendingInvoice
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
