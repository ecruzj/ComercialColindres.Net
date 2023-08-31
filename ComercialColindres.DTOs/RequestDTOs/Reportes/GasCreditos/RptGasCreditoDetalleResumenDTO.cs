using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    public class RptGasCreditoDetalleResumenDTO : BaseDTO
    {
        public List<RptGasCreditoEncabezadoDTO> GasCreditoEncabezado { get; set; }
        public List<RptOrdenesCombustibleOperativoDTO> OrdenesCombustibleOperativo { get; set; }
        public List<RptOrdenesCombustiblePersonalesDTO> OrdenesCombustiblePersonales { get; set; }
    }
}
