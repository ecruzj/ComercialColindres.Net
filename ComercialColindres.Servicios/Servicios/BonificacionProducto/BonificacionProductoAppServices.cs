using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.Bonificaciones;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class BonificacionProductoAppServices : IBonificacionProductoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public BonificacionProductoAppServices(ICacheAdapter cacheAdapter, ComercialColindresContext unidadDeTrabajo)
        {
            _cacheAdapter = cacheAdapter;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public BonificacionProductoDto GetBonificacion(GetBonificacionProducto request)
        {
            var cacheKey = string.Format("{0}{1}{2}-{3}", KeyCache.BonificacionProducto, nameof(GetBonificacionProducto), request.PlantaId, request.CategoriaProductoId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.BonifacionProducto.FirstOrDefault(b => b.PlantaId == request.PlantaId && b.CategoriaProductoId == request.CategoriaProductoId);
                var dtos = AutomapperTypeAdapter.ProyectarComo<BonificacionProducto, BonificacionProductoDto>(datos);
                return dtos;
            });
            return retorno;
        }
    }
}
