using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class ReportePagoDescargadoresResumen : BaseDTO
    {
        public List<ReportePagoDescargadoresDTO> PagoDescargadores { get; set; }
        public List<ReporteDescargadoresDTO> Descargas { get; set; }
    }
}
