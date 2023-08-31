using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs
{
    [Route("/sub-plantas/", "GET")]
    public class GetSubPlantasByValue : RequestBase, IReturn<List<SubPlantaDto>>
    {
        public int PlantaId { get; set; }
        public string ValorBusqueda { get; set; }
    }
}
