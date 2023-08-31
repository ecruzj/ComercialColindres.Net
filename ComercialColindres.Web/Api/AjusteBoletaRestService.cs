using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class AjusteBoletaRestService : IService
    {
        readonly IAjusteBoletaAppServices _ajusteBoletaAppServices;

        public AjusteBoletaRestService(IAjusteBoletaAppServices ajusteBoletaAppServices)
        {
            _ajusteBoletaAppServices = ajusteBoletaAppServices;
        }

        public object Get(GetByValorAjusteBoletas request)
        {
            return _ajusteBoletaAppServices.GetAjusteBoletas(request);
        }

        public object Post(PostAjusteBoleta request)
        {
            return _ajusteBoletaAppServices.CreateAjusteBoleta(request);
        }
        
        public object Delete(DeleteAjusteBoleta request)
        {
            return _ajusteBoletaAppServices.DeleteAjusteBoleta(request);
        }

        public object Post(PostActiveAjusteBoleta request)
        {
            return _ajusteBoletaAppServices.ActiveAjusteBoleta(request);
        }
    }
}