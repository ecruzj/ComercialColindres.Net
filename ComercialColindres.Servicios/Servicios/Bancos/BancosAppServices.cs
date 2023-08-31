using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class BancosAppServices : IBancosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public BancosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<BancosDTO> Get(GetAllBancos request)
        {
            var cacheKey = string.Format("{0}-{1}", KeyCache.Bancos, "GetAllBancos");
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.Bancos.ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Bancos>, IEnumerable<BancosDTO>>(datos);

                return datosDTO.ToList();
            });
            return datosConsulta;
        }
    }
}
