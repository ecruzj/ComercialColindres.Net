using ComercialColindres.DTOs.RequestDTOs.AjusteTipos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios.AjusteTipos
{
    public interface IAjusteTipoAppServices
    {
        List<AjusteTipoDto> GetAjusteTipos(GetAllAjusteTipos request);
    }
}
