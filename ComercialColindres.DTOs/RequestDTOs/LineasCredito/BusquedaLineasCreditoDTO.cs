using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCredito
{
    public class BusquedaLineasCreditoDTO : BusquedaResponseBaseDTO
    {
        public List<LineasCreditoDTO> Items { get; set; }
    }
}
