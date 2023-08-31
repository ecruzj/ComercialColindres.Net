using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible
{
    public class BusquedaOrdenesCombustibleDTO : BusquedaResponseBaseDTO
    {
        public List<OrdenesCombustibleDTO> Items { get; set; }
    }
}
