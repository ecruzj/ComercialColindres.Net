using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FacturaDetalleItemRestService : IService
    {
        IFacturaDetalleItemAppServices _facturaDetalleItemsAppServices;

        public FacturaDetalleItemRestService(IFacturaDetalleItemAppServices facturaDetalleItemsAppServices)
        {
            _facturaDetalleItemsAppServices = facturaDetalleItemsAppServices;
        }

        public object Get(GetDetailItemsByInvoiceId request)
        {
            return _facturaDetalleItemsAppServices.GetDetailItemsByInvoiceId(request);
        }

        public object Post(PostInvoiceDetailItems request)
        {
            return _facturaDetalleItemsAppServices.SaveInvoiceDetailItems(request);
        }
    }
}