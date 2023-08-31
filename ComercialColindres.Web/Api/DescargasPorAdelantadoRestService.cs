using ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class DescargasPorAdelantadoRestService : IService
    {
        IDescargasPorAdelantadoAppServices _descargasPorAdelantadoAppServices;

        public DescargasPorAdelantadoRestService(IDescargasPorAdelantadoAppServices descargasPorAdelantadoAppServices)
        {
            _descargasPorAdelantadoAppServices = descargasPorAdelantadoAppServices;
        }

        public object Get(GetDescargasAdelantadasPendientes request)
        {
            return _descargasPorAdelantadoAppServices.Get(request);
        }

        public object Get(GetDescargasAdelantadasPorPagoDescargadaId request)
        {
            return _descargasPorAdelantadoAppServices.Get(request);
        }

        public object Post(PostDescargasPorAdelantado request)
        {
            return _descargasPorAdelantadoAppServices.Post(request);
        }
    }
}