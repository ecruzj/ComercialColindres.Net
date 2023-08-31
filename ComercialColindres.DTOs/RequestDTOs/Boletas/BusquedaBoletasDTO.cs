using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Boletas
{
    public class BusquedaBoletasDTO : BusquedaResponseBaseDTO
    {
        public List<BoletasDTO> Items { get; set; }
    }
}
