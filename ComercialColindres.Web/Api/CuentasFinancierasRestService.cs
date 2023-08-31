using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CuentasFinancierasRestService : IService
    {
        ICuentasFinancierasAppServices _cuentasFinancierasAppServices;

        public CuentasFinancierasRestService(ICuentasFinancierasAppServices cuentasFinancierasAppServices)
        {
            _cuentasFinancierasAppServices = cuentasFinancierasAppServices;
        }

        public object Get(GetCuentasFinancierasPorBancoPorTipoCuenta request)
        {
            return _cuentasFinancierasAppServices.Get(request);
        }

        public object Get(GetCuentasFinancierasCajaChica request)
        {
            return _cuentasFinancierasAppServices.Get(request);
        }
    }
}