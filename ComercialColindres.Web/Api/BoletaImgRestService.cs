using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.Servicios.Servicios.BoletasImg;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class BoletaImgRestService : IService
    {
        IBoletaImgAppServices _boletaImgAppServices;

        public BoletaImgRestService(IBoletaImgAppServices boletaImgAppServices)
        {
            _boletaImgAppServices = boletaImgAppServices;
        }

        public object Get(GetBoletaImg request)
        {
            return _boletaImgAppServices.GetBoletaImagen(request);
        }
    }
}