using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class SubPlantaServices : ISubPlantaServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public SubPlantaServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<SubPlantaDto> GetSubPlantaByValue(GetSubPlantasByValue request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.SubPlantas, "GetSubPlantasPorValorBusqueda", request.PlantaId, request.ValorBusqueda);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.SubPlantas.Where(s => s.PlantaId == request.PlantaId && s.NombreSubPlanta.Contains(request.ValorBusqueda)).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<SubPlanta>, IEnumerable<SubPlantaDto>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }
    }
}
