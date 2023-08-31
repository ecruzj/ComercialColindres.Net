using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class SucursalesRestService : IService
    {
        ISucursalesAppServices _sucursalesAppServices;
        public SucursalesRestService(ISucursalesAppServices sucursalesAppServices)
        {
            _sucursalesAppServices = sucursalesAppServices;
        }

        public object Get(GetSucursal request)
        {
            return _sucursalesAppServices.Get(request);
        }

        public object Get(GetAllSucursales request)
        {
            return _sucursalesAppServices.Get(request);
        }

        public object Put(PutSucursal request)
        {
            return _sucursalesAppServices.Put(request);
        }
    }
}