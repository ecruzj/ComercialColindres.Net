using ComercialColindres.DTOs.RequestDTOs.PrecioDescargas;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPrecioDescargasAppServices
    {
        List<PrecioDescargasDTO> Get(GetPrecioDescargaPorPlantaId request);
        PrecioDescargasDTO Get(GetPrecioDescargaPorCategoriaEquipoId request);
    }
}
