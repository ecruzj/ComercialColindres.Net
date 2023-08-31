using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasBancarias
{
    [Route("/cuentas-bancarias/busqueda/{proveedorId}", "GET")]
    public class GetCuentasBancariasPorProveedorId : IReturn<List<CuentasBancariasDTO>>
    {
        public int ProveedorId { get; set; }
    }
    
    [Route("/cuentas-bancarias/", "POST")]
    public class PostCuentasBancarias : IReturn<ActualizarResponseDTO>
    {
        public int ProveedorId { get; set; }
        public List<CuentasBancariasDTO> CuentasBancarias { get; set; }
    }
    
}
