using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class BoletaHumedadModel : ObservableObject
    {
        public int BoletaHumedadId { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }
        public int PlantaId { get; set; }
        public int ProveedorId { get; set; }
        public bool BoletaIngresada { get; set; }
        public decimal HumedadPromedio { get; set; }
        public decimal PorcentajeTolerancia { get; set; }
        public DateTime FechaHumedad { get; set; }

        public int TotalBoletas
        {
            get { return _totalBoletas; }
            set
            {
                _totalBoletas = value;
                RaisePropertyChanged(nameof(TotalBoletas));
            }
        }
        private int _totalBoletas;
    }
}
