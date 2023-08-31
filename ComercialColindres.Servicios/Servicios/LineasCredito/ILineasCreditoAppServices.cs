using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ILineasCreditoAppServices
    {
        List<LineasCreditoDTO> Get(GetLineasCreditoPorBancoPorTipoCuenta request);
        List<LineasCreditoDTO> Get(GetLineasCreditoCajaChica request);
        BusquedaLineasCreditoDTO Get(GetByValorLineasCredito request);
        LineasCreditoDTO Get(GetLineaCreditoUltimo request);
        ActualizarResponseDTO Post(PostLineaCredito request);
        ActualizarResponseDTO Put(PutActivarLineaCredito request);
        ActualizarResponseDTO Put(PutLineaCredito request);
        ActualizarResponseDTO Put(PutLineaCreditoAnular request);
    }
}
