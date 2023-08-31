using ComercialColindres.DTOs.RequestDTOs.Bancos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IBancosAppServices
    {
        List<BancosDTO> Get(GetAllBancos request);
    }
}
