using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Cuadrillas
{
    [Route("/cuadrillas/busqueda/{plantaId}", "GET")]
    public class GetCuadrillasPorPlantaId : IReturn<List<CuadrillasDTO>>
    {
        public int PlantaId { get; set; }
    }

    [Route("/cuadrillas/busqueda-valorbusqueda", "GET")]
    public class GetCuadrillasPorValorBusqueda : IReturn<List<CuadrillasDTO>>
    {
        public int PlantaId { get; set; }
        public string ValorBusqueda { get; set; }
    }

    [Route("/cuadrillas/", "GET")]
    public class GetAllCuadrillas : BusquedaRequestBaseDTO, IReturn<BusquedaCuadrillasDTO>
    {
        public string ValorBusqueda { get; set; }
    }

    [Route("/cuadrillas/", "POST")]
    public class PostCuadrillas : IReturn<ActualizarResponseDTO>
    {
        public int PlantaId { get; set; }
        public List<CuadrillasDTO> Cuadrillas { get; set; }
    }
}
