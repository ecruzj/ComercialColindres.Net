using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado
{
    [Route("/descargas-por-adelantado/", "GET")]
    public class GetDescargasAdelantadasPendientes : RequestBase, IReturn<List<DescargasPorAdelantadoDTO>>
    {
    }

    [Route("/descargas-por-adelantado/{pagoDescargaId}", "GET")]
    public class GetDescargasAdelantadasPorPagoDescargadaId: RequestBase, IReturn<List<DescargasPorAdelantadoDTO>>
    {
        public int PagoDescargaId { get; set; }
    }

    [Route("/descargas-por-adelantado/", "POST")]
    public class PostDescargasPorAdelantado : RequestBase, IReturn<DescargasPorAdelantadoDTO>
    {
        public int PagoDescargaId { get; set; }
        public List<DescargasPorAdelantadoDTO> DescargasPorAdelantado { get; set; }
    }
}
