using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones
{
    public class BusquedaLineasCreditoDeduccionesDTO : BusquedaResponseBaseDTO
    {
        public List<LineasCreditoDeduccionesDTO> Items { get; set; }
    }
}
