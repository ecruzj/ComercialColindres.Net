using ComercialColindres.DTOs.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.Facturas
{
    public class BusquedaFacturasDTO : BusquedaResponseBaseDTO
    {
        public List<FacturasDTO> Items { get; set; }
    }
}
