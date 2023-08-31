using ComercialColindres.DTOs.RequestDTOs.PrecioProductos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PrecioProductosRestService : IService
    {
        IPrecioProductosAppServices _precioProductosAppServices;

        public PrecioProductosRestService(IPrecioProductosAppServices precioProductosAppServices)
        {
            _precioProductosAppServices = precioProductosAppServices;
        }

        public object Get(GetPrecioProductoPorPlantaId request)
        {
            return _precioProductosAppServices.Get(request);
        }

        public object Get(GetPrecioProductoPorCategoriaProductoId request)
        {
            return _precioProductosAppServices.Get(request);
        }
    }
}