using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Conductores
{
    [Route("/conductores/busqueda/{proveedorId}", "GET")]
    public class GetConductoresPorProveedorId : IReturn<List<ConductoresDTO>>
    {
        public int ProveedorId { get; set; }
    }

    [Route("/conductores/busqueda-valorbusqueda", "GET")]
    public class GetConductoresPorValorBusqueda : IReturn<List<ConductoresDTO>>
    {
        public int ProveedorId { get; set; }
        public string ValorBusqueda { get; set; }
    }

    [Route("/conductores/", "POST")]
    public class PostConductores : IReturn<ActualizarResponseDTO>
    {
        public int ProveedorId { get; set; }
        public List<ConductoresDTO> Conductores { get; set; }
    }    
}
