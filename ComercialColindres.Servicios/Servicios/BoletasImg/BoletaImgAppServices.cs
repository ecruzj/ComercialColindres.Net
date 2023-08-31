using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.Servicios.Clases;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios.BoletasImg
{
    public class BoletaImgAppServices : IBoletaImgAppServices
    {
        ComercialColindresContext _unitOfWork;

        public BoletaImgAppServices(ComercialColindresContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BoletaImgDto GetBoletaImagen(GetBoletaImg request)
        {
            var data = _unitOfWork.BoletasImg.FirstOrDefault(b => b.BoletaId == request.BoletaId);
            var datosDTO =
                AutomapperTypeAdapter.ProyectarComo<BoletaImg, BoletaImgDto>(data);

            return datosDTO;
        }
    }
}
