using ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ILineasCreditoDeduccionesAppServices
    {
        List<LineasCreditoDeduccionesDTO> Get(GetDeduccionesVariasPorLineaCreditoId request);
        List<LineasCreditoDeduccionesDTO> Get(GetDeduccionesOperativosPorLineaCreditoId request);
        ActualizarResponseDTO Post(PostDeduccionVarios request);
        ActualizarResponseDTO Put(PutDeduccionVarios request);
        EliminarResponseDTO Delete(DeleteDeduccionVarios request);
    }
}
