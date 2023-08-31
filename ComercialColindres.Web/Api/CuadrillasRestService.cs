using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class CuadrillasRestService : IService
    {
        ICuadrillasAppServices _cuadrillasAppServices;

        public CuadrillasRestService(ICuadrillasAppServices cuadrillasAppServices)
        {
            _cuadrillasAppServices = cuadrillasAppServices;
        }

        public object Get(GetAllCuadrillas request)
        {
            return _cuadrillasAppServices.Get(request);
        }

        public object Get(GetCuadrillasPorPlantaId request)
        {
            return _cuadrillasAppServices.Get(request);
        }

        public object Get(GetCuadrillasPorValorBusqueda request)
        {
            return _cuadrillasAppServices.Get(request);
        }

        public object Post(PostCuadrillas request)
        {
            return _cuadrillasAppServices.Post(request);
        }
    }
}