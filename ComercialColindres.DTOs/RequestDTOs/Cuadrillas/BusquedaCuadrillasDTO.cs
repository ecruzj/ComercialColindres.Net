using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Cuadrillas
{
    public class BusquedaCuadrillasDTO : BusquedaResponseBaseDTO
    {
        public List<CuadrillasDTO> Items { get; set; }
    }
}
