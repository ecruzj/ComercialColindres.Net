using ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaDeduccionManualRestService : IService
    {
        readonly IBoletaDeduccionManualAppServices _boletaDeduccionManualAppServices;
        public BoletaDeduccionManualRestService(IBoletaDeduccionManualAppServices boletaDeduccionManualAppServices)
        {
            _boletaDeduccionManualAppServices = boletaDeduccionManualAppServices;
        }

        public object Get(GetBoletaDeduccionesManualByBoletaId request)
        {
            return _boletaDeduccionManualAppServices.GetBoletaDeduccionesManualById(request);
        }

        public object Post(PostBoletadeduccionesManual request)
        {
            return _boletaDeduccionManualAppServices.SaveBoletaDeduccionesManual(request);
        }
    }
}