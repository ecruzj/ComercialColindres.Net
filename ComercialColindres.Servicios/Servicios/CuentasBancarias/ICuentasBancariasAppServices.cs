using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ICuentasBancariasAppServices
    {
        List<CuentasBancariasDTO> Get(GetCuentasBancariasPorProveedorId request);
        
        ActualizarResponseDTO Post(PostCuentasBancarias request);
    }
}
