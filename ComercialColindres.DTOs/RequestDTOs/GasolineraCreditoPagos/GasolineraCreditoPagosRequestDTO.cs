using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos
{
    [Route("/gas-creditospagos/busqueda-pagos/{gasCreditoId}", "GET")]
    public class GetGasCreditoPagosPorGasCreditoId : IReturn<List<GasolineraCreditoPagosDTO>>
    {
        public int GasCreditoId { get; set; }
    }

    [Route("/gas-creditospagos/", "POST")]
    public class PostGasCreditoPagos : IReturn<ActualizarResponseDTO>
    {
        public string UserId { get; set; }
        public int GasCreditoId { get; set; }
        public List<GasolineraCreditoPagosDTO> GasolineraCreditoPagos { get; set; }
    }
}
