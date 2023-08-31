using ComercialColindres.DTOs.RequestDTOs.FacturaDetalle;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FacturaDetalleRestService : IService
    {
        IFacturaDetalleBoletasAppServices _facturaDetalleAppSerives;
        public FacturaDetalleRestService(IFacturaDetalleBoletasAppServices facturaDetalleAppServices)
        {
            _facturaDetalleAppSerives = facturaDetalleAppServices;
        }

        public object Get(GetDetalleBoletasPorFacturaId request)
        {
            return _facturaDetalleAppSerives.Get(request);
        }

        public object Put(PutValidarDetalleBoletasMasivo request)
        {
            return _facturaDetalleAppSerives.Put(request);
        }

        public object Post(PostDetalleBoletas request)
        {
            return _facturaDetalleAppSerives.SaveInvoiceDetailBoletas(request);
        }
    }
}