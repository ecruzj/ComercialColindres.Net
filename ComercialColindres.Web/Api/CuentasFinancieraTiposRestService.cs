using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CuentasFinancieraTiposRestService : IService
    {
        ICuentasFinancieraTiposAppServices _cuentasFinancieraTiposAppServices;

        public CuentasFinancieraTiposRestService(ICuentasFinancieraTiposAppServices cuentasFinancieraTiposAppServices)
        {
            _cuentasFinancieraTiposAppServices = cuentasFinancieraTiposAppServices;
        }

        public object Get(GetAllTiposCuentasFinancieras request)
        {
            return _cuentasFinancieraTiposAppServices.Get(request);
        }
    }
}