using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OrdenesCompraDetalleBoletaRestService : IService
    {
        IOrdenesCompraDetalleBoletaAppServices _ordenesCompraDetalleBoletaAppServices;

        public OrdenesCompraDetalleBoletaRestService(IOrdenesCompraDetalleBoletaAppServices ordenesCompraDetalleBoletaAppServices)
        {
            _ordenesCompraDetalleBoletaAppServices = ordenesCompraDetalleBoletaAppServices;
        }

        public object Get(GetDetalleBoletasPorOrdenCompraId request)
        {
            return _ordenesCompraDetalleBoletaAppServices.Get(request);
        }
    }
}