using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class CuentasFinancierasAppServices : ICuentasFinancierasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public CuentasFinancierasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<CuentasFinancierasDTO> Get(GetCuentasFinancierasCajaChica request)
        {
            var cacheKey = string.Format("{0}-{1}-{2}", KeyCache.CuentasFinancieras, "GetCuentasFinancierasCajaChica", request.UsuarioId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuario = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
                var tienePermisoAdministrativo = usuario.TienePermisoAdministrativo();

                var datos = _unidadDeTrabajo.CuentasFinancieras.Where(b => b.CuentasFinancieraTipos.RequiereBanco == false
                                                                      && (b.EsCuentaAdministrativa == tienePermisoAdministrativo
                                                                          || b.EsCuentaAdministrativa == false));
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<CuentasFinancieras>, IEnumerable<CuentasFinancierasDTO>>(datos);

                return datosDTO.ToList();
            });
            return datosConsulta;
        }

        public List<CuentasFinancierasDTO> Get(GetCuentasFinancierasPorBancoPorTipoCuenta request)
        {
            var cacheKey = string.Format("{0}-{1}-{2}-{3}-{4}", KeyCache.CuentasFinancieras, "GetAllCuentasFinancieras", request.BancoId, 
                                                                request.UsuarioId, request.CuentaFinancieraTipoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuario = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
                var tienePermisoAdministrativo = usuario.TienePermisoAdministrativo();

                var datos = _unidadDeTrabajo.CuentasFinancieras.Where(b => b.BancoId == request.BancoId
                                                                      && b.CuentaFinancieraTipoId == request.CuentaFinancieraTipoId
                                                                      && (b.EsCuentaAdministrativa == tienePermisoAdministrativo
                                                                          || b.EsCuentaAdministrativa == false));
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<CuentasFinancieras>, IEnumerable<CuentasFinancierasDTO>>(datos);

                return datosDTO.ToList();
            });
            return datosConsulta;
        }
    }
}
