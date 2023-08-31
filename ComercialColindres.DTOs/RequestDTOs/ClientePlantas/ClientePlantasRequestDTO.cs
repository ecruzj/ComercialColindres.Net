using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.ClientePlantas
{
    [Route("/plantas/{plantaId}", "GET")]
    public class GetPlanta : RequestBase, IReturn<ClientePlantasDTO>
    {
        public int PlantaId { get; set; }
    }

    [Route("/plantas/por-valorbusqueda", "GET")]
    public class GetPlantasPorValorBusqueda : IReturn<List<ClientePlantasDTO>>
    {
        public string ValorBusqueda { get; set; }
    }

    [Route("/plantas/", "PUT")]
    public class PutPlanta : IReturn<ActualizarResponseDTO>
    {
        public ClientePlantasDTO Planta { get; set; }
    }

    [Route("/plantas/", "GET")]
    public class GetAllPlantas : IReturn<List<ClientePlantasDTO>>
    {
    }
}
