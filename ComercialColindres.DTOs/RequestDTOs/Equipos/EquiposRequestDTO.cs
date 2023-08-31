using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Equipos
{
    [Route("/equipos/busqueda/{proveedorId}", "GET")]
    public class GetEquiposPorProveedorId : IReturn<List<EquiposDTO>>
    {
        public int ProveedorId { get; set; }
    }

    [Route("/equipos/busqueda-valorbusqueda", "GET")]
    public class GetEquiposPorValorBusqueda : IReturn<List<EquiposDTO>>
    {
        public int ProveedorId { get; set; }
        public string ValorBusqueda { get; set; }
    }


    [Route("/equipos/", "POST")]
    public class PostEquipos : IReturn<ActualizarResponseDTO>
    {
        public int ProveedorId { get; set; }
        public List<EquiposDTO> Equipos { get; set; }
    }
}
