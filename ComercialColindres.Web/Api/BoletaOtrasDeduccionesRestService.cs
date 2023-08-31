using ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaOtrasDeduccionesRestService : IService
    {
        IBoletaOtrasDeduccionesAppServices _boletaOtrasDeduccionesAppServices;

        public BoletaOtrasDeduccionesRestService(IBoletaOtrasDeduccionesAppServices boletaOtrasDeduccionesAppServices)
        {
            _boletaOtrasDeduccionesAppServices = boletaOtrasDeduccionesAppServices;
        }

        public object Get(GetBoletaOtrasDeduccionesPorBoletaId request)
        {
            return _boletaOtrasDeduccionesAppServices.Get(request);
        }

        public object Post(PostBoletaOtrasDeducciones request)
        {
            return _boletaOtrasDeduccionesAppServices.Post(request);
        }
    }
}