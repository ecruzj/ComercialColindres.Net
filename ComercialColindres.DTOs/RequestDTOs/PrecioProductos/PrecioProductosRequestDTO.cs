using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PrecioProductos
{
    [Route("/precio-productos/busqueda/{plantaId}", "GET")]
    public class GetPrecioProductoPorPlantaId : IReturn<List<PrecioProductosDTO>>
    {
        public int PlantaId { get; set; }
    }

    [Route("/precio-productos/busqueda/", "GET")]
    public class GetPrecioProductoPorCategoriaProductoId : IReturn<PrecioProductosDTO>
    {
        public int PlantaId { get; set; }
        public int CategoriaProductoId { get; set; }
    }


    [Route("/precio-productos/", "POST")]
    public class PostPrecioProductos : IReturn<ActualizarResponseDTO>
    {
        public List<PrecioProductosDTO> Cuadrillas { get; set; }
    }
}
