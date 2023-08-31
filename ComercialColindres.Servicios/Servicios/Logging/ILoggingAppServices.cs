using ComercialColindres.DTOs.RequestDTOs.Logging;
using ComercialColindres.DTOs.ResponseDTOs;

namespace ComercialColindres.Servicios.Servicios
{
    public interface ILoggingAppServices
    {
        ActualizarResponseDTO Post(PostLogging request);
    }
}
