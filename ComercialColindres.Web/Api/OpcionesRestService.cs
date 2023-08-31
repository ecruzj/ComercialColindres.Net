using ComercialColindres.DTOs.RequestDTOs.Opciones;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class OpcionesRestService : IService
    {
        readonly IOpcionesAppServices _opcionesAppServices;

        public OpcionesRestService(IOpcionesAppServices opcionesAppServices)
        {
            this._opcionesAppServices = opcionesAppServices;
        }

        public object Get(FindOpciones request) => _opcionesAppServices.Get(request);

        public object Post(PostOpcion request) => _opcionesAppServices.Post(request);

        public object Put(PutOpcion request) => _opcionesAppServices.Put(request);

        public object Delete(DeleteOpcion request) => _opcionesAppServices.Delete(request);
    }
}