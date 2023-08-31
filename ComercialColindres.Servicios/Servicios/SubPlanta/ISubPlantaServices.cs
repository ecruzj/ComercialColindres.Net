using ComercialColindres.DTOs.RequestDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ISubPlantaServices
    {
        List<SubPlantaDto> GetSubPlantaByValue(GetSubPlantasByValue request);
    }
}
