using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteBoletas
{
    public class BusquedaAjusteBoletas : BusquedaResponseBaseDTO
    {
        public List<AjusteBoletaDto> Items { get; set; }
    }
}
