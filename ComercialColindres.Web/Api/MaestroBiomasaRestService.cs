using ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class MaestroBiomasaRestService : IService
    {
        IMaestroBiomasaAppServices _maestroBiomasaAppServices;

        public MaestroBiomasaRestService(IMaestroBiomasaAppServices maestroBiomasaAppServices)
        {
            _maestroBiomasaAppServices = maestroBiomasaAppServices;
        }

        public object Get(GetAllMaestroBiomasa request)
        {
            return _maestroBiomasaAppServices.Get(request);
        }
    }
}