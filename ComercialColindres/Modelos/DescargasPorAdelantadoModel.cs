using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class DescargasPorAdelantadoModel : ObservableObject
    {
        public int DescargaPorAdelantadoId { get; set; }
        public bool HasShippingNumber { get; set; }

        public string CodigoBoleta
        {
            get { return _codigoBoleta; }
            set
            {
                _codigoBoleta = value;
                RaisePropertyChanged("CodigoBoleta");
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

        public decimal PrecioDescarga
        {
            get { return _precioDescarga; }
            set
            {
                _precioDescarga = value;
                RaisePropertyChanged("PrecioDescarga");
            }
        }
        private decimal _precioDescarga;

        public int PlantaId { get; set; }
        public int PagoDescargaId { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }

        public bool PuedeEditarBoleta { get; set; }
        
        public decimal TotalDescargasPorAdelanto
        {
            get { return _totalDescargasPorAdelanto; }
            set
            {
                _totalDescargasPorAdelanto = value;
                RaisePropertyChanged("TotalDescargasPorAdelanto");
            }
        }
        private decimal _totalDescargasPorAdelanto;
        
        public decimal TotalPagoDescargas
        {
            get { return _totalPagoDescargas; }
            set
            {
                _totalPagoDescargas = value;
                RaisePropertyChanged("TotalPagoDescargas");
            }
        }
        private decimal _totalPagoDescargas;
        
        public int TotalDescargas
        {
            get { return _totalDescargas; }
            set
            {
                _totalDescargas = value;
                RaisePropertyChanged("TotalDescargas");
            }
        }
        private int _totalDescargas;

        public bool EsActualizacion { get; set; }
    }
}
