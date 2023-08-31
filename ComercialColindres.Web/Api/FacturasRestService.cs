using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FacturasRestService : IService
    {
        IFacturasAppServices _facturasAppServices;

        public FacturasRestService(IFacturasAppServices facturasAppServices)
        {
            _facturasAppServices = facturasAppServices;
        }

        public object Get(GetByValorFacturas request)
        {
            return _facturasAppServices.Get(request);
        }

        public object Post(PostFactura request)
        {
            return _facturasAppServices.Post(request);
        }

        public object Put(UpdateInfoInvoice request)
        {
            return _facturasAppServices.UpdateInvoice(request);
        }

        public object Put(PutFacturaAnular request)
        {
            return _facturasAppServices.Put(request);
        }

        public object Put(ActiveInvoiceById request)
        {
            return _facturasAppServices.ActiveInvoice(request);
        }
    }
}