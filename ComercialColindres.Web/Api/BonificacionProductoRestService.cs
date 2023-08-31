using ComercialColindres.DTOs.RequestDTOs.Bonificaciones;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BonificacionProductoRestService : IService
    {
        IBonificacionProductoAppServices _bonificacionProductoAppServices;

        public BonificacionProductoRestService(IBonificacionProductoAppServices bonificacionProductoAppServices)
        {
            _bonificacionProductoAppServices = bonificacionProductoAppServices;
        }

        public object Get(GetBonificacionProducto request)
        {
            return _bonificacionProductoAppServices.GetBonificacion(request);
        }
    }
}