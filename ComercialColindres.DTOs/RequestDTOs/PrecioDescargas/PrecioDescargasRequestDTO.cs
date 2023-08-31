using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.PrecioDescargas
{
    [Route("/precio-descargas/busqueda/{plantaId}", "GET")]
    public class GetPrecioDescargaPorPlantaId : IReturn<List<PrecioDescargasDTO>>
    {
        public int PlantaId { get; set; }
    }

    [Route("/precio-descargas/busqueda/", "GET")]
    public class GetPrecioDescargaPorCategoriaEquipoId : IReturn<PrecioDescargasDTO>
    {
        public int PlantaId { get; set; }
        public int EquipoCategoriaId { get; set; }
    }

    [Route("/precio-descargas/", "POST")]
    public class PostPrecioDescargas : IReturn<ActualizarResponseDTO>
    {
        public List<PrecioDescargasDTO> PrecioDescarga { get; set; }
    }
}
