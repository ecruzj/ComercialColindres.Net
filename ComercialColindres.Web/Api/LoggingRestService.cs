using ComercialColindres.DTOs.RequestDTOs.Logging;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class LoggingRestService : IService
    {
        ILoggingAppServices _loggingAppServices;

        public LoggingRestService(ILoggingAppServices loggingAppServices)
        {
            _loggingAppServices = loggingAppServices;
        }

        public object Post(PostLogging request)
        {
            return _loggingAppServices.Post(request);
        }
    }
}