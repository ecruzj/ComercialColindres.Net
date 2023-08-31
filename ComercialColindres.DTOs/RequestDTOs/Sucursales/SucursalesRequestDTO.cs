using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Sucursales
{
    [Route("/sucursales/{sucursalId}", "GET")]
    public class GetSucursal : IReturn<SucursalesDTO>
    {
        public int SucursalId { get; set; }
    }

    [Route("/sucursales/", "PUT")]
    public class PutSucursal : IReturn<ActualizarResponseDTO>
    {
        public SucursalesDTO Sucursal { get; set; }
    }

    [Route("/sucursales/", "GET")]
    public class GetAllSucursales : IReturn<List<SucursalesDTO>>
    {
    }
}
