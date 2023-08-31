using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class AjusteBoletaDetalleRestService : IService
    {
        IAjusteBoletaDetalleAppServices _ajusteBoletaDetalleAppServices;

        public AjusteBoletaDetalleRestService(IAjusteBoletaDetalleAppServices ajusteBoletaDetalleAppServices)
        {
            _ajusteBoletaDetalleAppServices = ajusteBoletaDetalleAppServices;
        }

        public object Get(GetAjusteBoletaDetalleByVendorId request)
        {
            return _ajusteBoletaDetalleAppServices.GetAjusteBoletaDetallado(request);
        }

        public object Get(GetAjusteBoletaDetalleByAjusteBoletaId request)
        {
            return _ajusteBoletaDetalleAppServices.GetAjusteBoletaDetalleByAjusteId(request);
        }

        public object Post(PostAjusteBoletaDetalle request)
        {
            return _ajusteBoletaDetalleAppServices.SaveAjusteBoletaDetalle(request);
        }
    }
}