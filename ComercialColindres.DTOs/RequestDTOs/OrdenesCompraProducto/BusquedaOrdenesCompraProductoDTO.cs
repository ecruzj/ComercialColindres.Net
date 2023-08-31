using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto
{
    public class BusquedaOrdenesCompraProductoDTO : BusquedaResponseBaseDTO
    {
        public List<OrdenesCompraProductoDTO> Items { get; set; }
    }
}
