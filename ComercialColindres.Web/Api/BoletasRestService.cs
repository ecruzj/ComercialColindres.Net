using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletasRestService : IService
    {
        IBoletasAppServices _boletasAppService;

        public BoletasRestService(IBoletasAppServices boletasAppService)
        {
            _boletasAppService = boletasAppService;
        }

        public object Get(GetBoletasPorValorBusqueda request)
        {
            return _boletasAppService.Get(request);
        }

        public object Get(GetBoletasPendientesDeFacturar request)
        {
            return _boletasAppService.Get(request);
        }

        public object Get(GetBoleta request)
        {
            return _boletasAppService.Get(request);
        }

        public object Get(GetByValorBoletas request)
        {
            return _boletasAppService.Get(request);
        }

        public object Get(GetBoletasInProcess request)
        {
            return _boletasAppService.Get(request);
        }

        public object Put(PutBoleta request)
        {
            return _boletasAppService.Put(request);
        }

        public object Put(PutCierreForzadoBoleta request)
        {
            return _boletasAppService.Put(request);
        }

        public object Put(OpenBoletaById request)
        {
            return _boletasAppService.OpenBoletaById(request);
        }

        public object Put(UpdateBoletaProperties request)
        {
            return _boletasAppService.UpdateBoletaProperties(request);
        }

        public object Post(PostBoleta request)
        {
            return _boletasAppService.Post(request);
        }

        public object Delete(DeleteBoleta request)
        {
            return _boletasAppService.Delete(request);
        }
    }
}