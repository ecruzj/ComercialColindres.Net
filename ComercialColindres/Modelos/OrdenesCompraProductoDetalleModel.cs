using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class OrdenesCompraProductoDetalleModel : ObservableObject
    {
        public int OrdenCompraProductoDetalleId { get; set; }
        public int OrdenCompraProductoId { get; set; }
        public int BiomasaId { get; set; }
        
        public decimal Toneladas
        {
            get { return _toneladas; }
            set
            {
                _toneladas = value;
                RaisePropertyChanged("Toneladas");
            }
        }
        private decimal _toneladas;
        
        public decimal PrecioDollar
        {
            get { return _precioDollar; }
            set
            {
                _precioDollar = value;
                RaisePropertyChanged("PrecioDollar");
            }
        }
        private decimal _precioDollar;
        
        public decimal ConversionDollarToLps
        {
            get { return _conversionDollarToLps; }
            set
            {
                _conversionDollarToLps = value;
                RaisePropertyChanged("ConversionDollarToLps");
            }
        }
        private decimal _conversionDollarToLps;

        public string MaestroBiomasaDescripcion
        {
            get { return _maestroBiomasaDescripcion; }
            set
            {
                _maestroBiomasaDescripcion = value;
                RaisePropertyChanged("MaestroBiomasaDescripcion");
            }
        }
        private string _maestroBiomasaDescripcion;

        public decimal TotalDollares
        {
            get { return _totalDollares; }
            set
            {
                _totalDollares = value;
                RaisePropertyChanged("TotalDollares");
            }
        }
        private decimal _totalDollares;
        
        public decimal TotalLps
        {
            get { return _totalLps; }
            set
            {
                _totalLps = value;
                RaisePropertyChanged("TotalLps");
            }
        }
        private decimal _totalLps;
        
        public decimal PrecioLPS
        {
            get { return _precioLPS; }
            set
            {
                _precioLPS = value;
                RaisePropertyChanged("PrecioLPS");
            }
        }
        private decimal _precioLPS;
    }
}
