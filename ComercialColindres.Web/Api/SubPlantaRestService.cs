using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class SubPlantaRestService : IService
    {
        ISubPlantaServices _subPlantaServices;

        public SubPlantaRestService(ISubPlantaServices subPlantaServices)
        {
            _subPlantaServices = subPlantaServices;
        }

        public object Get(GetSubPlantasByValue request)
        {
            return _subPlantaServices.GetSubPlantaByValue(request);
        }
    }
}