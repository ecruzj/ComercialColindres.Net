using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class ProveedoresRestService : IService
    {
        IProveedoresAppServices _proveedoresAppServices;

        public ProveedoresRestService(IProveedoresAppServices proveedoresAppServices)
        {
            _proveedoresAppServices = proveedoresAppServices;
        }

        public object Get(GetProveedor request)
        {
            return _proveedoresAppServices.Get(request);
        }

        public object Get(GetByValorProveedores request)
        {
            return _proveedoresAppServices.Get(request);
        }

        public object Get(GetProveedoresPorValorBusqueda request)
        {
            return _proveedoresAppServices.Get(request);
        }

        public object Put(PutProveedor request)
        {
            return _proveedoresAppServices.Put(request);
        }

        public object Post(PostProveedor request)
        {
            return _proveedoresAppServices.Post(request);
        }

        public object Delete(DeleteProveedor request)
        {
            return _proveedoresAppServices.Delete(request);
        }
    }
}