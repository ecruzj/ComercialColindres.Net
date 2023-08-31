using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ICuentasFinancieraTiposAppServices
    {
        List<CuentasFinancieraTiposDTO> Get(GetAllTiposCuentasFinancieras request);
    }
}
