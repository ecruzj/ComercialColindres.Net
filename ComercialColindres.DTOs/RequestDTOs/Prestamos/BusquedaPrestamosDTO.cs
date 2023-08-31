using ComercialColindres.DTOs.Clases;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Prestamos
{
    public class BusquedaPrestamosDTO : BusquedaResponseBaseDTO
    {
        public List<PrestamosDTO> Items { get; set; }
    }
}
