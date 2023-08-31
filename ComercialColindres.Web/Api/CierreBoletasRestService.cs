using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CierreBoletasRestService : IService
    {
        IBoletaCierresAppServices _boletaCierresAppServices;

        public CierreBoletasRestService(IBoletaCierresAppServices boletaCierresAppServices)
        {
            _boletaCierresAppServices = boletaCierresAppServices;
        }

        public object Get(GetBoletasCierrePorBoletaId request)
        {
            return _boletaCierresAppServices.Get(request);
        }

        public object Post(PostBoletasCierre request)
        {
            return _boletaCierresAppServices.Post(request);
        }

        public object Put(CloseBoletaMasive request)
        {
            return _boletaCierresAppServices.CloseBoletaMasive(request);
        }
    }
}