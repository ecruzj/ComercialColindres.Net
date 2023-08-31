using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IClientePlantasAppServices
    {
        ClientePlantasDTO Get(GetPlanta request);
        List<ClientePlantasDTO> Get(GetPlantasPorValorBusqueda request);
        ActualizarResponseDTO Put(PutPlanta request);
        List<ClientePlantasDTO> Get(GetAllPlantas request);
    }
}
