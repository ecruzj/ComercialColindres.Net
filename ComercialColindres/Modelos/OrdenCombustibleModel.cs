using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class OrdenCombustibleModel : ObservableObject
    {
        public int OrdenCombustibleId { get; set; }
        public string CodigoFactura { get; set; }
        public int GasCreditoId { get; set; }
        public string AutorizadoPor { get; set; }
        public int? BoletaId { get; set; }
        public int? ProveedorId { get; set; }
        public string PlacaEquipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsOrdenPersonal { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }


        public string FuelOrderSpecification
        {
            get { return _fuelOrderSpecification; }
            set
            {
                _fuelOrderSpecification = value;
                RaisePropertyChanged(nameof(FuelOrderSpecification));
            }
        }
        private string _fuelOrderSpecification;

        public decimal TotalAsignaciones
        {
            get { return _totalAsignaciones; }
            set
            {
                _totalAsignaciones = value;
                RaisePropertyChanged(nameof(TotalAsignaciones));
            }
        }
        private decimal _totalAsignaciones;

        public decimal BoletaSaldo
        {
            get { return _boletaSaldo; }
            set
            {
                _boletaSaldo = value;
                RaisePropertyChanged(nameof(BoletaSaldo));
            }
        }
        private decimal _boletaSaldo;
    }
}
