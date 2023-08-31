using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.RequestDTOs.AjusteTipos;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;

namespace ComercialColindres.Servicios.Servicios.AjusteTipos
{
    public class AjusteTipoAppServices : IAjusteTipoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public AjusteTipoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<AjusteTipoDto> GetAjusteTipos(GetAllAjusteTipos request)
        {
            var cacheKey = string.Format("{0}{1}", KeyCache.AjusteTipos, nameof(GetAllAjusteTipos));
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.AjusteTipos.ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<AjusteTipo>, IEnumerable<AjusteTipoDto>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }
    }
}
