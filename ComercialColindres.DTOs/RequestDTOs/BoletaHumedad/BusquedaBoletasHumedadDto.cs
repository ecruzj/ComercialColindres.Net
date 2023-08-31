using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs
{
    public class BusquedaBoletasHumedadDto : BusquedaResponseBaseDTO
    {
        public List<BoletaHumedadDto> Items { get; set; }
    }
}
