using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.EquiposCategorias
{
    [Route("/equipos-categorias/", "GET")]
    public class GetAllEquiposCategorias : IReturn<List<EquiposCategoriasDTO>>
    {
    }

    [Route("/equipos-categorias/porEquipoId", "GET")]
    public class GetEquipoCategoriaPorEquipoId : IReturn<EquiposCategoriasDTO>
    {
        public int EquipoId { get; set; }
    }

}
