using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ICuentasFinancierasAppServices
    {
        List<CuentasFinancierasDTO> Get(GetCuentasFinancierasPorBancoPorTipoCuenta request);
        List<CuentasFinancierasDTO> Get(GetCuentasFinancierasCajaChica request);
    }
}
