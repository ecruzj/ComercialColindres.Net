using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class NotasCreditoModel : ObservableObject
    {
        public int NotaCreditoId { get; set; }
        public int FacturaId { get; set; }

        

        public string NotaCreditoNo
        {
            get { return _notaCreditoNo; }
            set
            {
                _notaCreditoNo = value;
                RaisePropertyChanged(nameof(NotaCreditoNo));
            }
        }
        private string _notaCreditoNo;

        public decimal Monto
        {
            get { return _monto; }
            set
            {
                _monto = value;
                RaisePropertyChanged(nameof(Monto));
            }
        }
        private decimal _monto;

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                _observaciones = value;
                RaisePropertyChanged(nameof(Observaciones));
            }
        }
        private string _observaciones;

        public decimal TotalNotasCredito
        {
            get { return _totalNotasCredito; }
            set
            {
                _totalNotasCredito = value;
                RaisePropertyChanged(nameof(TotalNotasCredito));
            }
        }
        private decimal _totalNotasCredito;
    }
}
