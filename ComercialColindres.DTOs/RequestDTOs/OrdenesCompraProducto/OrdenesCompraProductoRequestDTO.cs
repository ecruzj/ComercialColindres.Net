using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto
{
    [Route("/ordenes-compra-producto/buscar-por-valor", "GET")]
    public class GetByValorOrdenesComprasProducto : BusquedaRequestBaseDTO, IReturn<BusquedaOrdenesCompraProductoDTO>
    {
    }

    [Route("/ordenes-compra-producto/por-planta/{plantaId}", "GET")]
    public class GetDatosPOPorPlantaId : IReturn<OrdenesCompraProductoDTO>
    {
        public int PlantaId { get; set; }
    }

    [Route("/ordenes-compra-producto/", "POST")]
    public class PostOrdenCompraProducto : IReturn<ActualizarResponseDTO>
    {
        public OrdenesCompraProductoDTO OrdenCompraProducto { get; set; }
        public string UsuarioId { get; set; }
    }

    [Route("/ordenes-compra-producto/", "PUT")]
    public class PutOrdenCompraProducto : IReturn<ActualizarResponseDTO>
    {
        public OrdenesCompraProductoDTO OrdenCompraProducto { get; set; }
        public string UserId { get; set; }
    }

    [Route("/ordenes-compra-producto/activar", "PUT")]
    public class PutActivarOrdenCompraProducto : IReturn<ActualizarResponseDTO>
    {
        public int OrdenCompraProductoId { get; set; }
        public int PlantaId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/ordenes-compra-producto/cerrar", "PUT")]
    public class PutCerrarOrdenCompraProducto : IReturn<ActualizarResponseDTO>
    {
        public int OrdenCompraProductoId { get; set; }
        public string UserId { get; set; }
    }

    [Route("/ordenes-compra-producto/", "DELETE")]
    public class DeleteOrdenCompraProducto : IReturn<EliminarResponseDTO>
    {
        public int OrdenCompraProductoId { get; set; }
        public string UserId { get; set; }
    }
}
