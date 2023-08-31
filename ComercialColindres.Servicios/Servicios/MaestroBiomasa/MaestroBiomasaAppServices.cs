using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class MaestroBiomasaAppServices : IMaestroBiomasaAppServices
    {
        ComercialColindresContext _unidadDeTrabajo;

        public MaestroBiomasaAppServices(ComercialColindresContext unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public List<MaestroBiomasaDTO> Get(GetAllMaestroBiomasa request)
        {
            var datos = _unidadDeTrabajo.MaestroBiomasa.ToList();
            var dto = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<MaestroBiomasa>, IEnumerable<MaestroBiomasaDTO>>(datos);
            return dto.ToList();
        }
    }
}
