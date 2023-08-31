using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos
{
    [Route("/ajuste-boleta-pagos/{ajusteBoletaDetalleId}", "GET")]
    public class GetAjusteBoletaPagoByDetailId : RequestBase, IReturn<List<AjusteBoletaPagoDto>>
    {
        public int AjusteBoletaDetalleId { get; set; }
    }

    [Route("/ajuste-boleta-pagos/by-boletaId/{boletaId}", "GET")]
    public class GetAjusteBoletaPagoByBoletaId : RequestBase, IReturn<List<AjusteBoletaPagoDto>>
    {
        public int BoletaId { get; set; }
    }

    [Route("/ajuste-boleta-pagos/", "POST")]
    public class PostAjusteBoletaPagoByBoleta : RequestBase, IReturn<AjusteBoletaPagoDto>
    {
        public int BoletaId { get; set; }
        public List<AjusteBoletaPagoDto> AjusteBoletaPagos { get; set; }
    }
}
