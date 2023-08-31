using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OrdenesCompraProductoRestService : IService
    {
        IOrdenesCompraProductoAppServices _ordenesCompraProductoAppServices;

        public OrdenesCompraProductoRestService(IOrdenesCompraProductoAppServices ordenesCompraProductoAppServices)
        {
            _ordenesCompraProductoAppServices = ordenesCompraProductoAppServices;
        }

        public object Get(GetByValorOrdenesComprasProducto request)
        {
            return _ordenesCompraProductoAppServices.Get(request);
        }

        public object Get(GetDatosPOPorPlantaId request)
        {
            return _ordenesCompraProductoAppServices.Get(request);
        }

        public object Post(PostOrdenCompraProducto request)
        {
            return _ordenesCompraProductoAppServices.Post(request);
        }

        public object Put(PutActivarOrdenCompraProducto request)
        {
            return _ordenesCompraProductoAppServices.Put(request);
        }

        public object Put(PutCerrarOrdenCompraProducto request)
        {
            return _ordenesCompraProductoAppServices.Put(request);
        }

        public object Put(PutOrdenCompraProducto request)
        {
            return _ordenesCompraProductoAppServices.Put(request);
        }

        public object Delete(DeleteOrdenCompraProducto request)
        {
            return _ordenesCompraProductoAppServices.Delete(request);
        }
    }
}