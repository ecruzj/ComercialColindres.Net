using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.DataCore.Logging;
using ComercialColindres.DTOs.RequestDTOs.Logging;
using ComercialColindres.DTOs.ResponseDTOs;
using System;

namespace ComercialColindres.Servicios.Servicios
{
    public class LoggingAppServices : ILoggingAppServices
    {
        ComercialColindresContext _unidadDeTrabajo;

        public LoggingAppServices(ComercialColindresContext unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public ActualizarResponseDTO Post(PostLogging request)
        {
            if (request == null) throw new ArgumentNullException("request");

            LoggerFactory.CreateLog().LogError(request.ErrorMessage);

            return new ActualizarResponseDTO();
        }
    }
}
