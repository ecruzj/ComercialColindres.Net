using ComercialColindres.DTOs.ResponseDTOs;
using ServiceStack.ServiceHost;

namespace ComercialColindres.DTOs.RequestDTOs.Logging
{
    [Route("/logging/", "POST")]
    public class PostLogging : IReturn<ActualizarResponseDTO>
    {
        public string ErrorMessage { get; set; }
    }
}
