using ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class LineasCreditoDeducciones : IService
    {
        ILineasCreditoDeduccionesAppServices _lineasCreditoDeduccionesAppServices;

        public LineasCreditoDeducciones(ILineasCreditoDeduccionesAppServices lineasCreditoDeduccionesAppServices)
        {
            _lineasCreditoDeduccionesAppServices = lineasCreditoDeduccionesAppServices;
        }

        public object Get(GetDeduccionesVariasPorLineaCreditoId request)
        {
            return _lineasCreditoDeduccionesAppServices.Get(request);
        }

        public object Get(GetDeduccionesOperativosPorLineaCreditoId request)
        {
            return _lineasCreditoDeduccionesAppServices.Get(request);
        }

        public object Post(PostDeduccionVarios request)
        {
            return _lineasCreditoDeduccionesAppServices.Post(request);
        }

        public object Put(PutDeduccionVarios request)
        {
            return _lineasCreditoDeduccionesAppServices.Put(request);
        }

        public object Delete(DeleteDeduccionVarios request)
        {
            return _lineasCreditoDeduccionesAppServices.Delete(request);
        }
    }
}