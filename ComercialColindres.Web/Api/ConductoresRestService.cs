using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class ConductoresRestService : IService
    {
        IConductoresAppServices _conductoresAppServices;

        public ConductoresRestService(IConductoresAppServices conductoresAppServices)
        {
            _conductoresAppServices = conductoresAppServices;
        }

        public object Get(GetConductoresPorProveedorId request)
        {
            return _conductoresAppServices.Get(request);
        }

        public object Get(GetConductoresPorValorBusqueda request)
        {
            return _conductoresAppServices.Get(request);
        }

        public object Post(PostConductores request)
        {
            return _conductoresAppServices.Post(request);
        }
    }
}