using ComercialColindres.DTOs.RequestDTOs.FacturasCategorias;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class FacturaCategoriaRestService : IService
    {
        IFacturasCategoriasAppServices _facturasCategoriasAppServices;

        public FacturaCategoriaRestService(IFacturasCategoriasAppServices facturasCategoriasAppServices)
        {
            _facturasCategoriasAppServices = facturasCategoriasAppServices;
        }

        public object Get(GetAllFacturasCategorias request)
        {
            return _facturasCategoriasAppServices.Get(request);
        }
    }
}