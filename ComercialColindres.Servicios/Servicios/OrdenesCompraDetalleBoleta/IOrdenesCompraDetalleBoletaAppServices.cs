using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IOrdenesCompraDetalleBoletaAppServices
    {
        List<OrdenesCompraDetalleBoletaDTO> Get(GetDetalleBoletasPorOrdenCompraId request);
    }
}
