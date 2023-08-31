using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class RecibosRestService : IService
    {
        IRecibosAppServices _recibosAppServices;
        public RecibosRestService(IRecibosAppServices recibosAppServices)
        {
            _recibosAppServices = recibosAppServices;
        }
    }
}