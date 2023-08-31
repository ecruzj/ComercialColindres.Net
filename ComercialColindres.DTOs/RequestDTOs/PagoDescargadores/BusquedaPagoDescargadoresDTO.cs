using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.PagoDescargadores
{
    public class BusquedaPagoDescargadoresDTO : BusquedaResponseBaseDTO
    {
        public List<PagoDescargadoresDTO> Items { get; set; }
    }
}
