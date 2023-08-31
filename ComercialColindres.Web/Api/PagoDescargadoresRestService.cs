using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class PagoDescargadoresRestService : IService
    {
        IPagoDescargadoresAppServices _pagoDescargadoresAppServices;

        public PagoDescargadoresRestService(IPagoDescargadoresAppServices pagoDescargadoresAppServices)
        {
            _pagoDescargadoresAppServices = pagoDescargadoresAppServices;
        }

        public object Post(PostPagosDescargas request)
        {
            return _pagoDescargadoresAppServices.Post(request);
        }

        public object Get(GetPagoDescargadoresUltimo request)
        {
            return _pagoDescargadoresAppServices.Get(request);
        }

        public object Get(GetByValorPagosDescargas request)
        {
            return _pagoDescargadoresAppServices.Get(request);
        }

        public object Put(PutPagoDescargas request)
        {
            return _pagoDescargadoresAppServices.Put(request);
        }

        public object Delete(DeletePagoDescargas request)
        {
            return _pagoDescargadoresAppServices.Delete(request);
        }
    }
}