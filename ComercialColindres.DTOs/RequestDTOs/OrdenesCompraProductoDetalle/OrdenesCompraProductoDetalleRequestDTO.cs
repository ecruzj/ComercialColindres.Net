using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle
{
    [Route("/ordenes-compra-producto-detalle/busqueda-orden-compra/{ordenCompraProductoId}", "GET")]
    public class GetOrdenesCompraProductoDetallePorPO : IReturn<List<OrdenesCompraProductoDetalleDTO>>
    {
        public int OrdenCompraProductoId { get; set; }
    }

    [Route("/ordenes-compra-producto-detalle/", "POST")]
    public class PostOrdenesCompraProductoDetalle : IReturn<ActualizarResponseDTO>
    {
        public int OrdenCompraProductoId { get; set; }
        public List<OrdenesCompraProductoDetalleDTO> OrdenesCompraProductoDetalle { get; set; }
        public string UserId { get; set; }
    }
}
