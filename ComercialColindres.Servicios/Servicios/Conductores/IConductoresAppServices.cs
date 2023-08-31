using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IConductoresAppServices
    {
        List<ConductoresDTO> Get(GetConductoresPorProveedorId request);
        List<ConductoresDTO> Get(GetConductoresPorValorBusqueda request);
        ActualizarResponseDTO Post(PostConductores request);
    }
}
