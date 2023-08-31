using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending
{
    public class RptBoletaPaymentPendingResumenDto
    {
        public RptBoletaPaymentPendingResumenDto()
        {
            BoletasPending = new List<RptBoletaPaymentPendingDto>();
        }

        public int VendorId { get; set; }
        public string VendorName { get; set; }

        public decimal SaldoPendiente
        {
            get { return _saldoPendiente; }
            set { _saldoPendiente = value; }
        }
        private decimal _saldoPendiente;

        public int BoletaQuantity
        {
            get { return _boletaQuantity; }
            set { _boletaQuantity = value; }
        }
        private int _boletaQuantity;

        public List<RptBoletaPaymentPendingDto> BoletasPending { get; set; }
        
        public List<int> GetBoletasToPay()
        {
            if (BoletasPending == null) return new List<int>();
            
            return BoletasPending.Where(c => c.IsChecked).Select(b => b.BoletaId).Distinct().ToList();
        }

        public bool IsPartialPayment()
        {
            if (BoletasPending == null) return false;

            return BoletasPending.Any(c => c.IsChecked);
        }

        public void GetValuesForPayment()
        {
            if (BoletasPending == null) return;
            bool hasChecked = BoletasPending.Any(c => c.IsChecked);

            SaldoPendiente = BoletasPending.Where(c => c.IsChecked == hasChecked).Sum(p => p.TotalPagar);
            BoletaQuantity = BoletasPending.Where(c => c.IsChecked == hasChecked).ToList().Count;
        }
    }
}
