using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IGasolineraCreditoPagosAppServices
    {
        List<GasolineraCreditoPagosDTO> Get(GetGasCreditoPagosPorGasCreditoId request);
        ActualizarResponseDTO Post(PostGasCreditoPagos request);
    }
}
