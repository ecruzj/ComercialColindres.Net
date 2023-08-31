using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Opciones
{
    [Route("/opciones/buscar", "GET")]
    public class FindOpciones : IReturn<List<OpcionesDTO>>
    {
        public string Filtro { get; set; }
    }

    [Route("/opciones/", "POST")]
    public class PostOpcion : IReturn<ActualizarResponseDTO>
    {
        public OpcionesDTO Opcion { get; set; }
    }

    [Route("/opciones/", "PUT")]
    public class PutOpcion : IReturn<ActualizarResponseDTO>
    {
        public OpcionesDTO Opcion { get; set; }
    }

    [Route("/opciones/", "DELETE")]
    public class DeleteOpcion : IReturn<EliminarResponseDTO>
    {
        public int OpcionId { get; set; }
    }
}
