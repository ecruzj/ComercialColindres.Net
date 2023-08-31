using System;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending
{
    public class RptBoletaPaymentPendingDto
    {
        public int BoletaId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Biomasa { get; set; }
        public decimal PesoProducto { get; set; }
        public decimal CantidadPenalizada { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal DeduccionTotal { get; set; }
        public decimal TotalPagar { get; set; }
        public string Planta { get; set; }
        public bool IsChecked { get; set; }
        public DateTime FechaSalida { get; set; }
    }
}