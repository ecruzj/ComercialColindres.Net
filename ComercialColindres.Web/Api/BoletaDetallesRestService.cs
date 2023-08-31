using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaDetallesRestService : IService
    {
        IBoletaDetallesAppServices _boletaDetallesAppServices;

        public BoletaDetallesRestService(IBoletaDetallesAppServices boletaDetallesAppServices)
        {
            _boletaDetallesAppServices = boletaDetallesAppServices;
        }

        public object Get(GetBoletasDetallePorBoletaId request)
        {
            return _boletaDetallesAppServices.Get(request);
        }
    }
}