using ComercialColindres.DTOs.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos
{
    public class CuentasFinancieraTiposDTO : BaseDTO
    {
        public int CuentaFinancieraTipoId { get; set; }
        public string Descripcion { get; set; }
        public bool RequiereBanco { get; set; }
    }
}
