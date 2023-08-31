using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle
{
    public class AjusteBoletaDetalleDto : BaseDTO
    {
        public int AjusteBoletaDetalleId { get; set; }
        public int AjusteBoletaId { get; set; }
        public int AjusteTipoId { get; set; }
        public decimal Monto { get; set; }
        public int? LineaCreditoId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        
        public string AjusteTipoDescripcion { get; set; }
        public string Estado { get; set; }
        public string CodigoBoleta { get; set; }
        public string NumeroEnvio { get; set; }

        public decimal SaldoPendiente
        {
            get { return _saldoPendiente; }
            set { _saldoPendiente = value; }
        }
        private decimal _saldoPendiente;

        public string CadenaAjuste { get; set; }

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
            }
        }
        private string _noDocumento;

        public string CodigoLineaCredito
        {
            get { return _codigoLineaCredito; }
            set
            {
                _codigoLineaCredito = value;
            }
        }
        private string _codigoLineaCredito;
    }
}
