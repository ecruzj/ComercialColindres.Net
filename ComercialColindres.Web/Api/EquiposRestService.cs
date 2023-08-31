using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class EquiposRestService : IService
    {
        IEquiposAppServices _equiposAppServices;

        public EquiposRestService(IEquiposAppServices equiposAppServices)
        {
            _equiposAppServices = equiposAppServices;
        }

        public object Get(GetEquiposPorProveedorId request)
        {
            return _equiposAppServices.Get(request);
        }

        public object Get(GetEquiposPorValorBusqueda request)
        {
            return _equiposAppServices.Get(request);
        }

        public object Post(PostEquipos request)
        {
            return _equiposAppServices.Post(request);
        }
    }
}