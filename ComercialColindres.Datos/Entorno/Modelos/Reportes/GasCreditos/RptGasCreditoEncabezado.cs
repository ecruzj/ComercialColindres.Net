using System;

namespace ComercialColindres.Datos.Entorno.Modelos
{
    public class RptGasCreditoEncabezado
    {
        public string Gasolinera { get; set; }
        public string CreadoPor { get; set; }
        public string Estado { get; set; }
        public bool EsCreditoActual { get; set; }
        public string CodigoGasCredito { get; set; }
        public decimal Credito { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
    }
}
