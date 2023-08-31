using ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IMaestroBiomasaAppServices
    {
        List<MaestroBiomasaDTO> Get(GetAllMaestroBiomasa request);
    }
}
