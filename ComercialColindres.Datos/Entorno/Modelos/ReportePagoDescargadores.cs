using System;
using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.Modelos
{
    public class ReportePagoDescargadores
    {
        public string FormaDePago { get; set; }
        public string NombreBanco { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }
}
