using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class FacturaDetalleBoletasModel : ObservableObject
    {
        public int BoletaId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public string Motorista { get; set; }
        public decimal PesoProducto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal PrecioProductoCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaSalida { get; set; }
        public string PlacaEquipo { get; set; }
        public string DescripcionTipoProducto { get; set; }
        public string DescripcionTipoEquipo { get; set; }
        public string NombreProveedor { get; set; }
        
        public decimal TotalPrecioBoleta
        {
            get { return _totalPrecioBoleta; }
            set
            {
                _totalPrecioBoleta = value;
                RaisePropertyChanged("TotalPrecioBoleta");
            }
        }
        private decimal _totalPrecioBoleta;
        
        public decimal TotalPago
        {
            get { return _totalPago; }
            set
            {
                _totalPago = value;
                RaisePropertyChanged("TotalPago");
            }
        }
        private decimal _totalPago;

        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                _unitPrice = value;
                RaisePropertyChanged("UnitPrice");
            }
        }
        private decimal _unitPrice;
    }
}
