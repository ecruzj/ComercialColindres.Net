using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class ReportePagoDescargadoresDTO
    {
        public string FormaDePago { get; set; }
        public string NombreBanco { get; set; }
        public string NoDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }
}
