using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ICuadrillasAppServices
    {
        List<CuadrillasDTO> Get(GetCuadrillasPorPlantaId request);
        BusquedaCuadrillasDTO Get(GetAllCuadrillas request);
        List<CuadrillasDTO> Get(GetCuadrillasPorValorBusqueda request);
        ActualizarResponseDTO Post(PostCuadrillas request);
    }
}
