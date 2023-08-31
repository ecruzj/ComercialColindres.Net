using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PrecioProductos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Servicios.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class PrecioProductosAppServices : IPrecioProductosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public PrecioProductosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public PrecioProductosDTO Get(GetPrecioProductoPorCategoriaProductoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.PrecioProductos, "GetPrecioProductoPorCategoriaProductoId", request.PlantaId, request.CategoriaProductoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                PrecioProductos dato = _unidadDeTrabajo.PrecioProductos.FirstOrDefault(p => p.PlantaId == request.PlantaId && 
                                                                                       p.CategoriaProductoId == request.CategoriaProductoId);
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarComo<PrecioProductos, PrecioProductosDTO>(dato);
                return datosDTO;
            });

            return datosConsulta;
        }

        public List<PrecioProductosDTO> Get(GetPrecioProductoPorPlantaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.PrecioProductos, "GetPrecioProductoPorPlantaId", request.PlantaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PrecioProductos> datos = _unidadDeTrabajo.PrecioProductos.Where(p => p.PlantaId == request.PlantaId).OrderByDescending(o => o.PrecioCompra).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PrecioProductos>, IEnumerable<PrecioProductosDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
    }
}
