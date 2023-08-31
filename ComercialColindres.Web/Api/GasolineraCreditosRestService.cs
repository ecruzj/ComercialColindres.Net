using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class GasolineraCreditosRestService : IService
    {
        IGasolineraCreditosAppServices _gasolineraCreditosAppServices;

        public GasolineraCreditosRestService(IGasolineraCreditosAppServices gasolineraCreditosAppServices)
        {
            _gasolineraCreditosAppServices = gasolineraCreditosAppServices;
        }

        public object Get(GetGasolineraCreditoUltimo request)
        {
            return _gasolineraCreditosAppServices.Get(request);
        }

        public object Get(GetGasolineraCreditoActual request)
        {
            return _gasolineraCreditosAppServices.Get(request);
        }

        public object Get(GetByValorGasCreditos request)
        {
            return _gasolineraCreditosAppServices.Get(request);
        }

        public object Post(PostGasolineraCreditos request)
        {
            return _gasolineraCreditosAppServices.Post(request);
        }

        public object Put(PutActivarGasolineraCreditos request)
        {
            return _gasolineraCreditosAppServices.Put(request);
        }

        public object Put(PutGasolineraCreditos request)
        {
            return _gasolineraCreditosAppServices.Put(request);
        }

        public object Delete(DeleteGasolineraCredito request)
        {
            return _gasolineraCreditosAppServices.Delete(request);
        }
    }
}