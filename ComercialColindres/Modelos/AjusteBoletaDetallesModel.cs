using GalaSoft.MvvmLight;
using System;

namespace ComercialColindres.Modelos
{
    public class AjusteBoletaDetallesModel : ObservableObject
    {
        public int AjusteBoletaDetalleId { get; set; }
        public int AjusteBoletaId { get; set; }
        public int AjusteTipoId { get; set; }
        public string Estado { get; set; }

        public int BancoId { get; set; }

        public string FormaDePago
        {
            get
            {
                return _formaDePago;
            }
            set
            {
                _formaDePago = value;
                RaisePropertyChanged(nameof(FormaDePago));
            }
        }
        private string _formaDePago;

        public string NombreBanco
        {
            get
            {
                return _nombreBanco;
            }
            set
            {
                _nombreBanco = value;
                RaisePropertyChanged(nameof(NombreBanco));
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
                RaisePropertyChanged(nameof(NoDocumento));
            }
        }
        private string _noDocumento;

        public int? LineaCreditoId { get; set; }

        public string CodigoLineaCredito
        {
            get { return _codigoLineaCredito; }
            set
            {
                _codigoLineaCredito = value;
                RaisePropertyChanged(nameof(CodigoLineaCredito));
            }
        }
        private string _codigoLineaCredito;

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

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                _observaciones = value;
                RaisePropertyChanged(nameof(Observaciones));
            }
        }
        private string _observaciones;

        public DateTime FechaCreacion
        {
            get { return _fechaCreacion; }
            set
            {
                _fechaCreacion = value;
                RaisePropertyChanged(nameof(FechaCreacion));
            }
        }
        private DateTime _fechaCreacion;
        
        public string ModificadoPor
        {
            get { return _modificadoPor; }
            set
            {
                _modificadoPor = value;
                RaisePropertyChanged(nameof(ModificadoPor));
            }
        }
        private string _modificadoPor;

        public string AjusteTipoDescripcion
        {
            get { return _ajusteTipoDescripcion; }
            set
            {
                _ajusteTipoDescripcion = value;
                RaisePropertyChanged(nameof(AjusteTipoDescripcion));
            }
        }
        private string _ajusteTipoDescripcion;
    }
}
