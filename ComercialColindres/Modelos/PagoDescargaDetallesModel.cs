using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class PagoDescargaDetallesModel : ObservableObject
    {
        public int PagoDescargaDetalleId { get; set; }
        public int PagoDescargaId { get; set; }
        public string FormaDePago
        {
            get
            {
                return _formaDePago;
            }
            set
            {
                _formaDePago = value;
                RaisePropertyChanged("FormaDePago");
            }
        }
        private string _formaDePago;

        public int LineaCreditoId { get; set; }
        public string CodigoLineaCredito
        {
            get { return _codigoLineaCredito; }
            set
            {
                _codigoLineaCredito = value;
                RaisePropertyChanged("CodigoLineaCredito");
            }
        }
        private string _codigoLineaCredito;

        public bool PuedeEditarPagoDescargaDetalle
        {
            get { return _puedeEditarPagoDescargaDetalle; }
            set
            {
                _puedeEditarPagoDescargaDetalle = value;
                RaisePropertyChanged("PuedeEditarPagoDescargaDetalle");
            }
        }
        private bool _puedeEditarPagoDescargaDetalle;

        public int BancoId { get; set; }
        public bool EsEdicion { get; set; }

        public string NoDocumento
        {
            get
            {
                return _noDocumento;
            }
            set
            {
                _noDocumento = value;
                RaisePropertyChanged("NoDocumento");
            }
        }
        private string _noDocumento;

        public decimal Monto
        {
            get
            {
                return _monto;
            }
            set
            {
                _monto = value;
                RaisePropertyChanged("Monto");
            }
        }
        private decimal _monto;

        public decimal CantidadJustificada
        {
            get
            {
                return _cantidadJustificada;
            }
            set
            {
                _cantidadJustificada = value;
                RaisePropertyChanged("CantidadJustificada");
            }
        }
        private decimal _cantidadJustificada;

        public string NombreBanco
        {
            get
            {
                return _nombreBanco;
            }
            set
            {
                _nombreBanco = value;
                RaisePropertyChanged("NombreBanco");
            }
        }
        private string _nombreBanco;
    }
}
