using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FacturaPagoRestService : IService
    {
        IFacturaPagoAppServices _facturaPagoAppServices;

        public FacturaPagoRestService(IFacturaPagoAppServices facturaPagoAppServices)
        {
            _facturaPagoAppServices = facturaPagoAppServices;
        }

        public object Get(GetPagosByInvoiceId request)
        {
            return _facturaPagoAppServices.GetInvoicePaymentByInvoiceId(request);
        }

        public object Post(PostInvoicePagos request)
        {
            return _facturaPagoAppServices.SaveInvoicePayment(request);
        }
    }
}