using ComercialColindres.DTOs.RequestDTOs.EquiposCategorias;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class EquiposCategoriasRestService : IService
    {
        IEquiposCategoriasAppServices _equiposCategoriasAppServices;
        public EquiposCategoriasRestService(IEquiposCategoriasAppServices equiposCategoriasAppServices)
        {
            _equiposCategoriasAppServices = equiposCategoriasAppServices;
        }

        public object Get(GetAllEquiposCategorias request)
        {
            return _equiposCategoriasAppServices.Get(request);
        }

        public object Get(GetEquipoCategoriaPorEquipoId request)
        {
            return _equiposCategoriasAppServices.Get(request);
        }
    }
    
}