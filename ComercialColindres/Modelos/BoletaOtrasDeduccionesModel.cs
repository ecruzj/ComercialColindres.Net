using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class BoletaOtrasDeduccionesModel : ObservableObject
    {
        public int BoletaOtraDeduccionId { get; set; }
        public int BoletaId { get; set; }

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

        public int? LineaCreditoId { get; set; }
        
        public bool RequiereLineaCredito
        {
            get { return _requiereLineaCredito; }
            set
            {
                _requiereLineaCredito = value;
                RaisePropertyChanged("RequiereLineaCredito");
            }
        }
        private bool _requiereLineaCredito;
        
        public bool IsNegativeDeduction
        {
            get { return isNegativeDeduction; }
            set
            {
                isNegativeDeduction = value;
                RaisePropertyChanged(nameof(IsNegativeDeduction));
            }
        }
        private bool isNegativeDeduction;

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
        
        public bool PuedeEliminarBoletaOtraDeduccion
        {
            get { return _puedeEliminarBoletaOtraDeduccion; }
            set
            {
                _puedeEliminarBoletaOtraDeduccion = value;
                RaisePropertyChanged("PuedeEliminarBoletaOtraDeduccion");
            }
        }
        private bool _puedeEliminarBoletaOtraDeduccion;

        public int BancoId { get; set; }

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
        
        public string MotivoDeduccion
        {
            get { return _motivoDeduccion; }
            set
            {
                _motivoDeduccion = value;
                RaisePropertyChanged("MotivoDeduccion");
            }
        }
        private string _motivoDeduccion;

        public bool EsDeduccionManual { get; set; }

        public decimal DeduccionTotal
        {
            get { return _deduccionTotal; }
            set
            {
                _deduccionTotal = value;
                RaisePropertyChanged("DeduccionTotal");
            }
        }
        private decimal _deduccionTotal;
        
        public decimal TotalPagarBoleta
        {
            get { return _totalPagarBoleta; }
            set
            {
                _totalPagarBoleta = value;
                RaisePropertyChanged("TotalPagarBoleta");
            }
        }
        private decimal _totalPagarBoleta;
        
        public decimal TotalIngreso
        {
            get { return _totalIngreso; }
            set
            {
                _totalIngreso = value;
                RaisePropertyChanged("TotalIngreso");
            }
        }
        private decimal _totalIngreso;
    }
}
