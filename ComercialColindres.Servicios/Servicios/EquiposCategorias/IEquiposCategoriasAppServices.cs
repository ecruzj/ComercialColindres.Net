using ComercialColindres.DTOs.RequestDTOs.EquiposCategorias;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IEquiposCategoriasAppServices
    {
        List<EquiposCategoriasDTO> Get(GetAllEquiposCategorias request);

        EquiposCategoriasDTO Get(GetEquipoCategoriaPorEquipoId request);
    }
}
