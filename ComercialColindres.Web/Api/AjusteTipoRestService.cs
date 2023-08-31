using ComercialColindres.DTOs.RequestDTOs.AjusteTipos;
using ComercialColindres.Servicios.Servicios.AjusteTipos;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class AjusteTipoRestService : IService
    {
        readonly IAjusteTipoAppServices _ajusteTipoAppServices;

        public AjusteTipoRestService(IAjusteTipoAppServices ajusteTipoAppServices)
        {
            _ajusteTipoAppServices = ajusteTipoAppServices;
        }

        public object Get(GetAllAjusteTipos request)
        {
            return _ajusteTipoAppServices.GetAjusteTipos(request);
        }
    }
}