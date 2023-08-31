using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos
{
    [Route("/cuentas-financiera-tipos/", "GET")]
    public class GetAllTiposCuentasFinancieras : IReturn<List<CuentasFinancieraTiposDTO>>
    {
    }
}
