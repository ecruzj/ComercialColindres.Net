using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta
{
    [Route("/ordenes-compra-detalle-boleta/busqueda-boletas/{ordenCompraId}", "GET")]
    public class GetDetalleBoletasPorOrdenCompraId : IReturn<List<OrdenesCompraDetalleBoletaDTO>>
    {
        public int OrdenCompraId { get; set; }
    }
}
