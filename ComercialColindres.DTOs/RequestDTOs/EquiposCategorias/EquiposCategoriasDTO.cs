using ComercialColindres.DTOs.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.DTOs.RequestDTOs.EquiposCategorias
{
    public class EquiposCategoriasDTO : BaseDTO
    {
        public int EquipoCategoriaId { get; set; }
        public string Descripcion { get; set; }
    }
}
