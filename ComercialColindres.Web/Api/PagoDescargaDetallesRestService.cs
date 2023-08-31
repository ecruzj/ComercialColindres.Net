using ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PagoDescargaDetallesRestService : IService
    {
        IPagoDescargaDetallesAppServices _pagoDescargaDetallesAppServices;

        public PagoDescargaDetallesRestService(IPagoDescargaDetallesAppServices pagoDescargaDetallesAppServices)
        {
            _pagoDescargaDetallesAppServices = pagoDescargaDetallesAppServices;
        }

        public object Get(GetPagoDescargasDetallePorPagoDescargaId request)
        {
            return _pagoDescargaDetallesAppServices.Get(request);
        }

        public object Post(PostPagoDescargasDetalle request)
        {
            return _pagoDescargaDetallesAppServices.Post(request);
        }
    }
}