using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class CategoriaProductosAppServices : ICategoriaProductosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public CategoriaProductosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<CategoriaProductosDTO> Get(GetCategoriaProductoPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}-{3}", KeyCache.CategoriaProductos, "GetCategoriaProductoPorValorBusqueda", request.ValorBusqueda, request.PlantaId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = CategoriaProductosEspecificaciones.FiltrarProductosPorPlanta(request.ValorBusqueda, request.PlantaId);
                var datos = _unidadDeTrabajo.CategoriaProductos.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<CategoriaProductos>, IEnumerable<CategoriaProductosDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        void RemoverCacheCategoriaProductos()
        {
            var cacheKey = string.Format("{0}", KeyCache.CategoriaProductos);
            _cacheAdapter.Remove(cacheKey);
        }

    }
}
