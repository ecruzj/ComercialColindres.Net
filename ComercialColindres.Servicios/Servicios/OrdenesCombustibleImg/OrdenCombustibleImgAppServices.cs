using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.Servicios.Clases;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios.OrdenesCombustibleImg
{
    public class OrdenCombustibleImgAppServices : IOrdenCombustibleImgAppServices
    {
        ComercialColindresContext _unitOfWork;
        public OrdenCombustibleImgAppServices(ComercialColindresContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrdenCombustibleImgDto GetOrdenCombustibleImg(GetOrdenCombustibleImg request)
        {
            var data = _unitOfWork.OrdenesCombustibleImg.FirstOrDefault(o => o.OrdenCombustibleId == request.OrdenCombustibleId);
            var datosDTO =
                AutomapperTypeAdapter.ProyectarComo<OrdenCombustibleImg, OrdenCombustibleImgDto>(data);

            return datosDTO;
        }
    }
}
