using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class ClientePlantasRestService : IService
    {
        IClientePlantasAppServices _clientePlantasAppServices;

        public ClientePlantasRestService(IClientePlantasAppServices clientePlantasAppServices)
        {
            _clientePlantasAppServices = clientePlantasAppServices;
        }

        public object Get(GetPlanta request)
        {
            return _clientePlantasAppServices.Get(request);
        }

        public object Get(GetPlantasPorValorBusqueda request)
        {
            return _clientePlantasAppServices.Get(request);
        }

        public object Get(GetAllPlantas request)
        {
            return _clientePlantasAppServices.Get(request);
        }

        public object Put(PutPlanta request)
        {
            return _clientePlantasAppServices.Put(request);
        }
    }
}