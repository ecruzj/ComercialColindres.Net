using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class AjusteBoletaPagoRestService : IService
    {
        readonly IAjusteBoletaPagoAppServices _ajusteBoletaPagoAppServices;

        public AjusteBoletaPagoRestService(IAjusteBoletaPagoAppServices ajusteBoletaPagoAppServices)
        {
            _ajusteBoletaPagoAppServices = ajusteBoletaPagoAppServices;
        }

        public object Get(GetAjusteBoletaPagoByBoletaId request)
        {
            return _ajusteBoletaPagoAppServices.GetAjusteBoletaPagoByBoletaId(request);
        }

        public object Get(GetAjusteBoletaPagoByDetailId request)
        {
            return _ajusteBoletaPagoAppServices.GetAjusteBoletaPagoByDetailId(request);
        }

        public object Post(PostAjusteBoletaPagoByBoleta request)
        {
            return _ajusteBoletaPagoAppServices.SaveAjusteBoletaPagos(request);
        }
    }
}