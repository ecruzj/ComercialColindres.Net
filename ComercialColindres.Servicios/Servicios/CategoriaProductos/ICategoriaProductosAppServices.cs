using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ICategoriaProductosAppServices
    {
        List<CategoriaProductosDTO> Get(GetCategoriaProductoPorValorBusqueda request);
    }
}
