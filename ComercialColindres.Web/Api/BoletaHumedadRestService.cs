using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaHumedadRestService : IService
    {
        IBoletaHumedadAppServices _boletaHumedadAppServices;

        public BoletaHumedadRestService(IBoletaHumedadAppServices boletaHumedadAppServices)
        {
            _boletaHumedadAppServices = boletaHumedadAppServices;
        }

        public object Get(GetByValorBoletasHumedad request)
        {
            return _boletaHumedadAppServices.GetBoletasHumedadPaged(request);
        }

        public object Put(PutBoletasHumedad request)
        {
            return _boletaHumedadAppServices.CreateBoletasHumedad(request);
        }

        public object Delete(DeleteBoletasHumedad request)
        {
            return _boletaHumedadAppServices.DeleteBoletaHumedad(request);
        }
    }
}