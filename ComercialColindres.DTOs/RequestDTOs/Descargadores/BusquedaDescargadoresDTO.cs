using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Descargadores
{
    public class BusquedaDescargadoresDTO : BusquedaResponseBaseDTO
    {
        public List<DescargadoresDTO> Items { get; set; }
    }
}
