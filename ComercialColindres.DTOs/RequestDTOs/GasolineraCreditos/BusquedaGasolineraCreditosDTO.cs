using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos
{
    public class BusquedaGasolineraCreditosDTO : BusquedaResponseBaseDTO
    {
        public List<GasolineraCreditosDTO> Items { get; set; }
    }
}
