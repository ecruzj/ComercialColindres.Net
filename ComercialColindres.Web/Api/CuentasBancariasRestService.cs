using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CuentasBancariasRestService : IService
    {
        ICuentasBancariasAppServices _cuentasBancariasAppServices;

        public CuentasBancariasRestService(ICuentasBancariasAppServices cuentasBancariasAppServices)
        {
            _cuentasBancariasAppServices = cuentasBancariasAppServices;
        }

        public object Get(GetCuentasBancariasPorProveedorId request)
        {
            return _cuentasBancariasAppServices.Get(request);
        }
        
        public object Post(PostCuentasBancarias request)
        {
            return _cuentasBancariasAppServices.Post(request);
        }
    }
}