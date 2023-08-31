using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class GasolineraCreditoPagosRestService : IService
    {
        IGasolineraCreditoPagosAppServices _gasolineraCreditoPagosAppServices;

        public GasolineraCreditoPagosRestService(IGasolineraCreditoPagosAppServices gasolineraCreditoPagosAppServices)
        {
            _gasolineraCreditoPagosAppServices = gasolineraCreditoPagosAppServices;
        }

        public object Get(GetGasCreditoPagosPorGasCreditoId request)
        {
            return _gasolineraCreditoPagosAppServices.Get(request);
        }

        public object Post(PostGasCreditoPagos request)
        {
            return _gasolineraCreditoPagosAppServices.Post(request);
        }
    }
}