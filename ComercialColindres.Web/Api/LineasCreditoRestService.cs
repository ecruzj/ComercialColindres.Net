using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class LineasCreditoRestService : IService
    {
        ILineasCreditoAppServices _lineasCreditoAppServices;

        public LineasCreditoRestService(ILineasCreditoAppServices lineasCreditoAppServices)
        {
            _lineasCreditoAppServices = lineasCreditoAppServices;
        }

        public object Get(GetLineasCreditoPorBancoPorTipoCuenta request)
        {
            return _lineasCreditoAppServices.Get(request);
        }

        public object Get(GetByValorLineasCredito request)
        {
            return _lineasCreditoAppServices.Get(request);
        }

        public object Get(GetLineaCreditoUltimo request)
        {
            return _lineasCreditoAppServices.Get(request);
        }

        public object Get(GetLineasCreditoCajaChica request)
        {
            return _lineasCreditoAppServices.Get(request);
        }

        public object Post(PostLineaCredito request)
        {
            return _lineasCreditoAppServices.Post(request);
        }

        public object Put(PutActivarLineaCredito request)
        {
            return _lineasCreditoAppServices.Put(request);
        }

        public object Put(PutLineaCredito request)
        {
            return _lineasCreditoAppServices.Put(request);
        }

        public object Put(PutLineaCreditoAnular request)
        {
            return _lineasCreditoAppServices.Put(request);
        }
    }
}