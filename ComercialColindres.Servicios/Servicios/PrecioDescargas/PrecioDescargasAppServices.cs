using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PrecioDescargas;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Servicios.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class PrecioDescargasAppServices : IPrecioDescargasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public PrecioDescargasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public PrecioDescargasDTO Get(GetPrecioDescargaPorCategoriaEquipoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.PrecioDescargas, "GetPrecioDescargaPorCategoriaEquipoId", request.PlantaId, request.EquipoCategoriaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                PrecioDescargas dato = _unidadDeTrabajo.PrecioDescargas.FirstOrDefault(p => p.PlantaId == request.PlantaId &&
                                                                                       p.EquipoCategoriaId == request.EquipoCategoriaId);
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarComo<PrecioDescargas, PrecioDescargasDTO>(dato);
                return datosDTO;
            });

            return datosConsulta;
        }

        public List<PrecioDescargasDTO> Get(GetPrecioDescargaPorPlantaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.PrecioDescargas, "GetPrecioProductoPorPlantaId", request.PlantaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PrecioDescargas> datos = _unidadDeTrabajo.PrecioDescargas.Where(p => p.PlantaId == request.PlantaId).OrderBy(o => o.PrecioDescarga).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PrecioDescargas>, IEnumerable<PrecioDescargasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
    }
}
