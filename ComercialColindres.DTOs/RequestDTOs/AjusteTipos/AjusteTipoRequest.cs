using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.AjusteTipos
{
    [Route("/ajuste-tipos/por-valorbusqueda", "GET")]
    public class GetAllAjusteTipos : RequestBase, IReturn<List<AjusteTipoDto>>
    {
    }
}
