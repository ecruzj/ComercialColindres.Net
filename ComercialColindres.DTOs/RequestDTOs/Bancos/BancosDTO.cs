using ComercialColindres.DTOs.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.Bancos
{
    public class BancosDTO : BaseDTO
    {
        public int BancoId { get; set; }
        public string Descripcion { get; set; }
    }
}
