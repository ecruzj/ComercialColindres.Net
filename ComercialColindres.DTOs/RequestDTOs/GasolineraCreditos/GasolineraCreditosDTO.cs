using ComercialColindres.DTOs.Clases;
using System;

namespace ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos
{
    public class GasolineraCreditosDTO : BaseDTO
    {
        public int GasCreditoId { get; set; }
        public string CodigoGasCredito { get; set; }
        public int GasolineraId { get; set; }
        public decimal Credito { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal SaldoAnterior { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public bool EsCreditoActual { get; set; }

        public string NombreGasolinera { get; set; }
        public decimal Debito { get; set; }
    }
}
