using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BancosRestService : IService
    {
        IBancosAppServices _bancosAppServices;
        public BancosRestService(IBancosAppServices bancosAppServices)
        {
            _bancosAppServices = bancosAppServices;
        }

        public object Get(GetAllBancos request)
        {
            return _bancosAppServices.Get(request);
        }
    }
}