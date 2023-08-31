using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OrdenesCompraProductoDetalleRestService : IService
    {
        IOrdenesCompraProductoDetalleAppServices _ordenesCompraProductoDetalleAppServices;

        public OrdenesCompraProductoDetalleRestService(IOrdenesCompraProductoDetalleAppServices ordenesCompraProductoDetalleAppServices)
        {
            _ordenesCompraProductoDetalleAppServices = ordenesCompraProductoDetalleAppServices;
        }

        public object Get(GetOrdenesCompraProductoDetallePorPO request)
        {
            return _ordenesCompraProductoDetalleAppServices.Get(request);
        }

        public object Post(PostOrdenesCompraProductoDetalle request)
        {
            return _ordenesCompraProductoDetalleAppServices.Post(request);
        }
    }
}