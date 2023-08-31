using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Modelos
{
    public class PagoPrestamoModel : ObservableObject
    {
        public int PagoPrestamoId { get; set; }
        public int PrestamoId { get; set; }
        public string FormaDePago { get; set; }
        public int? BancoId { get; set; }
        public string NoDocumento { get; set; }
        public int? BoletaId { get; set; }
        
        public decimal MontoAbono
        {
            get
            {
                return _montoAbono;
            }
            set
            {
                _montoAbono = value;
                RaisePropertyChanged("MontoAbono");
            }
        }
        private decimal _montoAbono;
        
        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                _observaciones = value;
                RaisePropertyChanged("Observaciones");
            }
        }
        private string _observaciones;

        public string CodigoPrestamo { get; set; }
        public string CodigoBoleta { get; set; }
        public bool PuedeEditarAbonoPorBoleta { get; set; }
        public string PlantaDestino { get; set; }
        
        public string NombreBanco
        {
            get { return _nombreBanco; }
            set
            {
                _nombreBanco = value;
                RaisePropertyChanged("NombreBanco");            
            }
        }
        private string _nombreBanco;
                
        public decimal TotalDeuda
        {
            get
            {
                return _totalDeduda;
            }
            set
            {
                _totalDeduda = value;
            }
        }
        private decimal _totalDeduda;

        public decimal TotalAPagar
        {
            get { return _totalAPagar; }
            set
            {
                _totalAPagar = value;
                RaisePropertyChanged("TotalAPagar");
            }
        }
        private decimal _totalAPagar;
        
        public decimal SaldoPendiente
        {
            get { return _saldoPendiente; }
            set
            {
                _saldoPendiente = value;
                RaisePropertyChanged("SaldoPendiente");
            }
        }
        private decimal _saldoPendiente;

        public decimal TotalAbono
        {
            get { return _totalAbono; }
            set
            {
                _totalAbono = value;
                RaisePropertyChanged("TotalAbono");
            }
        }
        private decimal _totalAbono;

        public decimal TotalAPagarSinAbono
        {
            get
            {
                return _totalAPagarSinAbono;
            }
            set
            {
                _totalAPagarSinAbono = value;
            }
        }
        private decimal _totalAPagarSinAbono;
    }
}
