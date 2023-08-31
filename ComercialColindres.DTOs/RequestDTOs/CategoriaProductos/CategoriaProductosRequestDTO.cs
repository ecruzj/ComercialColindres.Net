using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.CategoriaProductos
{
    [Route("/categoria-productos/busqueda/por-valor", "GET")]
    public class GetCategoriaProductoPorValorBusqueda : IReturn<List<CategoriaProductosDTO>>
    {
        public string ValorBusqueda { get; set; }
        public int PlantaId { get; set; }
    }
}
