using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class FacturaPagoModel : ObservableObject
    {
        public int FacturaPagoId { get; set; }
        public int FacturaId { get; set; }
        public int BancoId { get; set; }
        public string Planta { get; set; }
        
        public DateTime FechaDePago
        {
            get { return _fechaDePago; }
            set
            {
                _fechaDePago = value;
                RaisePropertyChanged(nameof(FechaDePago));
            }
        }
        private DateTime _fechaDePago;

        public string FormaDePago
        {
            get { return _formaDePago; }
            set
            {
                _formaDePago = value;
                RaisePropertyChanged(nameof(FormaDePago));
            }
        }
        private string _formaDePago;

        public string ReferenciaBancaria
        {
            get { return _referenciaBancaria; }
            set
            {
                _referenciaBancaria = value;
                RaisePropertyChanged(nameof(ReferenciaBancaria));
            }
        }
        private string _referenciaBancaria;

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

        public string NombreBanco
        {
            get { return _nombreBanco; }
            set
            {
                _nombreBanco = value;
                RaisePropertyChanged(nameof(NombreBanco));
            }
        }
        private string _nombreBanco;

        public decimal TotalPaid
        {
            get { return _totalPaid; }
            set
            {
                _totalPaid = value;
                RaisePropertyChanged(nameof(TotalPaid));
            }
        }
        private decimal _totalPaid;
    }
}
