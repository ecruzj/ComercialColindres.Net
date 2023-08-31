using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PrestamosTransferenciasRestService : IService
    {
        IPrestamosTransferenciasAppServices _prestamosTransferenciasAppServices;

        public PrestamosTransferenciasRestService(IPrestamosTransferenciasAppServices prestamosTransferenciasAppServices)
        {
            _prestamosTransferenciasAppServices = prestamosTransferenciasAppServices;
        }

        public object Get(GetPrestamosTransferenciasPorPrestamoId request)
        {
            return _prestamosTransferenciasAppServices.Get(request);
        }

        public object Post(PostPrestamoTransferencias request)
        {
            return _prestamosTransferenciasAppServices.Post(request);
        }
    }
}