using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ISucursalesAppServices
    {
        SucursalesDTO Get(GetSucursal request);
        ActualizarResponseDTO Put(PutSucursal request);
        List<SucursalesDTO> Get(GetAllSucursales request);
    }
}
