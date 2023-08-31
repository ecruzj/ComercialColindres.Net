using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.DTOs.ResponseDTOs;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IOrdenesCompraProductoAppServices
    {
        BusquedaOrdenesCompraProductoDTO Get(GetByValorOrdenesComprasProducto request);
        OrdenesCompraProductoDTO Get(GetDatosPOPorPlantaId request);
        ActualizarResponseDTO Post(PostOrdenCompraProducto request);
        ActualizarResponseDTO Put(PutOrdenCompraProducto request);
        ActualizarResponseDTO Put(PutActivarOrdenCompraProducto request);
        ActualizarResponseDTO Put(PutCerrarOrdenCompraProducto request);
        EliminarResponseDTO Delete(DeleteOrdenCompraProducto request);
    }
}
