using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class AjusteBoletaPagoModel : ObservableObject
    {
        public int AjusteBoletaPagoId { get; set; }
        public int AjusteBoletaDetalleId { get; set; }
        public int BoletaId { get; set; }
        public DateTime FechaAbono { get; set; }

        public string CodigoBoleta
        {
            get { return _codigoBoleta; }
            set
            {
                _codigoBoleta = value;
                RaisePropertyChanged(nameof(CodigoBoleta));
            }
        }
        private string _codigoBoleta;

        public string NumeroEnvio
        {
            get { return _numeroEnvio; }
            set
            {
                _numeroEnvio = value;
                RaisePropertyChanged(nameof(NumeroEnvio));
            }
        }
        private string _numeroEnvio;

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

        public decimal TotalPayments
        {
            get { return _totalPayments; }
            set
            {
                _totalPayments = value;
                RaisePropertyChanged(nameof(TotalPayments));
            }
        }
        private decimal _totalPayments;

        public decimal SaldoBoleta
        {
            get { return _saldoBoleta; }
            set
            {
                _saldoBoleta = value;
                RaisePropertyChanged(nameof(SaldoBoleta));
            }
        }
        private decimal _saldoBoleta;
    }
}
