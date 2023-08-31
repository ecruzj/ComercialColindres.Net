using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Proveedores
{
    public class BusquedaProveedoresDTO : BusquedaResponseBaseDTO
    {
        public List<ProveedoresDTO> Items { get; set; }
    }
}
