using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras
{
    [Route("/cuentas-financieras/porbanco-portipocuenta", "GET")]
    public class GetCuentasFinancierasPorBancoPorTipoCuenta : IReturn<List<CuentasFinancierasDTO>>
    {
        public int BancoId { get; set; }
        public int UsuarioId { get; set; }
        public int CuentaFinancieraTipoId { get; set; }
    }

    [Route("/cuentas-financieras/caja-chica", "GET")]
    public class GetCuentasFinancierasCajaChica : IReturn<List<CuentasFinancierasDTO>>
    {
        public int UsuarioId { get; set; }
    }
}
