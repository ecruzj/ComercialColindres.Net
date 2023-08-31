using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PagoPrestamosRestService : IService
    {
        IPagoPrestamosAppServices _pagoPrestamosAppServices;

        public PagoPrestamosRestService(IPagoPrestamosAppServices pagoPrestamosAppServices)
        {
            _pagoPrestamosAppServices = pagoPrestamosAppServices;
        }

        public object Get(GetPagoPrestamosPorBoletaId request)
        {
            return _pagoPrestamosAppServices.Get(request);
        }

        public object Get(GetPagoPrestamos request)
        {
            return _pagoPrestamosAppServices.Get(request);
        }

        public object Post(PostPagosPrestamoPorBoletaId request)
        {
            return _pagoPrestamosAppServices.Post(request);
        }

        public object Post(PostOtrosAbonosPrestamo request)
        {
            return _pagoPrestamosAppServices.Post(request);
        }

        public object Put(PutAbonosPrestamoPorBoletas request)
        {
            return _pagoPrestamosAppServices.Put(request);
        }
    }
}