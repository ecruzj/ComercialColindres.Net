using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PrestamosRestService : IService
    {
        IPrestamosAppServices _prestamosAppServices;

        public PrestamosRestService(IPrestamosAppServices prestamosAppServices)
        {
            _prestamosAppServices = prestamosAppServices;
        }

        public object Get(GetByValorPrestamos request)
        {
            return _prestamosAppServices.Get(request);
        }

        public object Get(GetPrestamoUltimo request)
        {
            return _prestamosAppServices.Get(request);
        }

        public object Get(GetPrestamoPorProveedorId request)
        {
            return _prestamosAppServices.Get(request);
        }

        public object Post(PostPrestamo request)
        {
            return _prestamosAppServices.Post(request);
        }

        public object Put(PutPrestamo request)
        {
            return _prestamosAppServices.Put(request);
        }

        public object Put(PutActivarPrestamo request)
        {
            return _prestamosAppServices.Put(request);
        }

        public object Put(PutPrestamoAnular request)
        {
            return _prestamosAppServices.Put(request);
        }
    }
}