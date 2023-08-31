using ComercialColindres.DTOs.RequestDTOs.Gasolineras;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class GasolinerasRestService : IService
    {
        IGasolinerasAppServices _gasolinerasAppServices;

        public GasolinerasRestService(IGasolinerasAppServices gasolinerasAppServices)
        {
            _gasolinerasAppServices = gasolinerasAppServices;
        }

        public object Get(GetGasolinera request)
        {
            return _gasolinerasAppServices.Get(request);
        }

        public object Get(GetGasolinerasPorValorBusqueda request)
        {
            return _gasolinerasAppServices.Get(request);
        }

        public object Post(PostGasolinera request)
        {
            return _gasolinerasAppServices.Post(request);
        }

        public object Put(PutGasolinera request)
        {
            return _gasolinerasAppServices.Put(request);
        }
    }
}