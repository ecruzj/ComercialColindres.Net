using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.Servicios.Servicios.OrdenesCombustibleImg;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OrdenCombustibleImgRestService : IService
    {
        IOrdenCombustibleImgAppServices _ordenCombustibleImgAppServices;
        public OrdenCombustibleImgRestService(IOrdenCombustibleImgAppServices ordenCombustibleImgAppServices)
        {
            _ordenCombustibleImgAppServices = ordenCombustibleImgAppServices;
        }

        public object Get(GetOrdenCombustibleImg request)
        {
            return _ordenCombustibleImgAppServices.GetOrdenCombustibleImg(request);
        }
    }
}