using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.FacturasCategorias
{
    [Route("/facturas-categorias/", "GET")]
    public class GetAllFacturasCategorias : IReturn<List<FacturasCategoriasDTO>>
    {
    }
}
