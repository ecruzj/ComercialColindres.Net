using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaHumedadAsignacionRestService : IService
    {
        IBoletasHumedadAsignacionAppServices _boletasHumedadAsignacionAppServices;

        public BoletaHumedadAsignacionRestService(IBoletasHumedadAsignacionAppServices boletasHumedadAsignacionAppServices)
        {
            _boletasHumedadAsignacionAppServices = boletasHumedadAsignacionAppServices;
        }

        public object Get(GetBoletasHumedadByVendor request)
        {
            return _boletasHumedadAsignacionAppServices.GetBoletasHumidityByVendor(request);
        }
    }
}