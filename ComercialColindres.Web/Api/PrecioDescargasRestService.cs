using ComercialColindres.DTOs.RequestDTOs.PrecioDescargas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PrecioDescargasRestService : IService
    {
        public IPrecioDescargasAppServices _precioDescargarAppServices;

        public PrecioDescargasRestService(IPrecioDescargasAppServices precioDescargasAppServices)
        {
            _precioDescargarAppServices = precioDescargasAppServices;
        }

        public object Get(GetPrecioDescargaPorPlantaId request)
        {
            return _precioDescargarAppServices.Get(request);
        }

        public object Get(GetPrecioDescargaPorCategoriaEquipoId request)
        {
            return _precioDescargarAppServices.Get(request);
        }
    }
}