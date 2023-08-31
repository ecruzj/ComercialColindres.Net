using ComercialColindres.DTOs.RequestDTOs.PrecioProductos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IPrecioProductosAppServices
    {
        List<PrecioProductosDTO> Get(GetPrecioProductoPorPlantaId request);
        PrecioProductosDTO Get(GetPrecioProductoPorCategoriaProductoId request);
    }
}
