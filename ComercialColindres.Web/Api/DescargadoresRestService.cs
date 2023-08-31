using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class DescargadoresRestService : IService
    {
        private readonly IDescargadoresAppServices _descargadoresAppServices;

        public DescargadoresRestService(IDescargadoresAppServices descargadoresAppServices)
        {
            _descargadoresAppServices = descargadoresAppServices;
        }
        
        public object Get(GetDescargasPorPagoDescargaId request)
        {
            return _descargadoresAppServices.Get(request);
        }

        public object Get(GetDescargasPorCuadrillaId request)
        {
            return _descargadoresAppServices.Get(request);
        }

        public object Get(GetDescargasAplicaPagoPorCuadrillaId request)
        {
            return _descargadoresAppServices.Get(request);
        }

        public object Get(GetByValorDescargadores request)
        {
            return _descargadoresAppServices.Get(request);
        }

        public object Put(PutDescarga request)
        {
            return _descargadoresAppServices.Put(request);
        }

        public object Put(PutDescargaAnular request)
        {
            return _descargadoresAppServices.Put(request);
        }

        public object Post(PostDescargadores request)
        {
            return _descargadoresAppServices.Post(request);
        }
    }
}