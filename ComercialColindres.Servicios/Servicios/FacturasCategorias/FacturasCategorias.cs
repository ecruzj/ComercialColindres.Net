using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.FacturasCategorias;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Servicios.Servicios
{
    public class FacturasCategoriasAppServices : IFacturasCategoriasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public FacturasCategoriasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<FacturasCategoriasDTO> Get(GetAllFacturasCategorias request)
        {
            var cacheKey = string.Format("{0}-{1}", KeyCache.FacturasCategorias, "GetAllFacturasCategorias");
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.FacturasCategorias.ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<FacturasCategorias>, IEnumerable<FacturasCategoriasDTO>>(datos);

                return datosDTO.ToList();
            });
            return datosConsulta;
        }
    }
}
