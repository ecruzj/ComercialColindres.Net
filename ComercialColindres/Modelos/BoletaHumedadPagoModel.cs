using GalaSoft.MvvmLight;

namespace ComercialColindres.Modelos
{
    public class BoletaHumedadPagoModel : ObservableObject
    {
        public int BoletaHumedadPagoId { get; set; }
        public int BoletaId { get; set; }
        public int BoletaHumedadId { get; set; }
        
        public decimal TotalHumidityPayment
        {
            get { return _totalHumidityPayment; }
            set
            {
                _totalHumidityPayment = value;
                RaisePropertyChanged(nameof(TotalHumidityPayment));
            }
        }
        private decimal _totalHumidityPayment;
        
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

        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
    }
}
