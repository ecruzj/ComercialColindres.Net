using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.Bonificaciones
{
    [Route("/bonifiacion-producto/", "GET")]
    public class GetBonificacionProducto : RequestBase, IReturn<BonificacionProductoDto>
    {
        public int PlantaId { get; set; }
        public int CategoriaProductoId { get; set; }
    }
}
