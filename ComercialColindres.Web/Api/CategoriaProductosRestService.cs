using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CategoriaProductosRestService : IService
    {
        ICategoriaProductosAppServices _categoriaProductosAppServices;

        public CategoriaProductosRestService(ICategoriaProductosAppServices categoriaProductosAppServices)
        {
            _categoriaProductosAppServices = categoriaProductosAppServices;
        }

        public object Get(GetCategoriaProductoPorValorBusqueda request)
        {
            return _categoriaProductosAppServices.Get(request);
        }
    }
}