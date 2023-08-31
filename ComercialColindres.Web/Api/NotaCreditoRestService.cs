using ComercialColindres.DTOs.RequestDTOs.NotasCredito;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class NotaCreditoRestService : IService
    {
        INotaCreditoServices _notaCreditoServices;

        public NotaCreditoRestService(INotaCreditoServices notaCreditoServices)
        {
            _notaCreditoServices = notaCreditoServices;
        }

        public object Get(GetNotasCreditoByInvoiceId request)
        {
            return _notaCreditoServices.GetNotasCreditoByInvoiceId(request);
        }

        public object Post(PostNotasCredito request)
        {
            return _notaCreditoServices.SaveNotasCredito(request);
        }
    }
}