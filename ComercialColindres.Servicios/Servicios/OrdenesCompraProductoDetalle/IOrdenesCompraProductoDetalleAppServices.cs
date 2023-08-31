using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle;
using ComercialColindres.DTOs.ResponseDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IOrdenesCompraProductoDetalleAppServices
    {
        List<OrdenesCompraProductoDetalleDTO> Get(GetOrdenesCompraProductoDetallePorPO request);
        ActualizarResponseDTO Post(PostOrdenesCompraProductoDetalle request);
    }
}
