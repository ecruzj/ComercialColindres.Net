using System;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using ComercialColindres.Datos.Helpers;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class GasolineraCreditosAppServices : IGasolineraCreditosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        private const string _codigoCorrelativo_GasCredito = "GC";

        public GasolineraCreditosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }
        
        public BusquedaGasolineraCreditosDTO Get(GetByValorGasCreditos request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.GasCreditos, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = GasolineraCreditosEspecificaciones.FiltrarBusqueda(request.Filtro);
                List<GasolineraCreditos> datos = _unidadDeTrabajo.GasolineraCreditos.Where(especificacion.EvalFunc).OrderByDescending(o => o.CodigoGasCredito).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<GasolineraCreditos>, IEnumerable<GasolineraCreditosDTO>>(datosPaginados.Items as IEnumerable<GasolineraCreditos>);

                var dto = new BusquedaGasolineraCreditosDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<GasolineraCreditosDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public GasolineraCreditosDTO Get(GetGasolineraCreditoUltimo request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.GasCreditos, "GetGasolineraCreditoUltimo", request.SucursalId, request.Fecha.Year.ToString());
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var prefijoControl = _codigoCorrelativo_GasCredito + DateTime.Now.ToString("yy") + "-";
                var codigoGasCredito = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_GasCredito, prefijoControl, false);
                return new GasolineraCreditosDTO { CodigoGasCredito = codigoGasCredito };
            });

            return retorno;
        }

        public GasolineraCreditosDTO Get(GetGasolineraCreditoActual request)
        {
            var cacheKey = string.Format("{0}{1}", KeyCache.GasCreditos, "GetGasolineraCreditoActual");
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var codigoGasCreditoActual = _unidadDeTrabajo.GasolineraCreditos.Where(g => g.EsCreditoActual == true && g.Estado == Estados.ACTIVO)
                                                                                .FirstOrDefault();

                if (codigoGasCreditoActual == null)
                {
                    return null;
                }

                return new GasolineraCreditosDTO { CodigoGasCredito = codigoGasCreditoActual.CodigoGasCredito, GasCreditoId = codigoGasCreditoActual.GasCreditoId };
            });

            return retorno;
        }

        public ActualizarResponseDTO Post(PostGasolineraCreditos request)
        {
            var gasolineraCredito = _unidadDeTrabajo.GasolineraCreditos.Find(request.GasolineraCredito.GasCreditoId);

            if (gasolineraCredito != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasCreditoId YA Existe"
                };
            }

            var gasolinera = _unidadDeTrabajo.Gasolineras.Find(request.GasolineraCredito.GasolineraId);

            if (gasolinera == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasolineraId NO Existe"
                };
            }
            
            gasolineraCredito = new GasolineraCreditos("??", gasolinera.GasolineraId, request.GasolineraCredito.Credito, request.GasolineraCredito.SaldoActual, request.GasolineraCredito.FechaInicio, request.UserId);

            var mensajesValidacion = gasolineraCredito.GetValidationErrors().ToList();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            gasolineraCredito.CodigoGasCredito = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_GasCredito, string.Empty, true);
            _unidadDeTrabajo.GasolineraCreditos.Add(gasolineraCredito);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CreacionGasolineraCreditos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheGasolineraCreditos();

            return new ActualizarResponseDTO();
        }
        
        public ActualizarResponseDTO Put(PutActivarGasolineraCreditos request)
        {
            var gasCredito = _unidadDeTrabajo.GasolineraCreditos.Find(request.GasCreditoId);

            if (gasCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasCreditoId NO Existe"
                };
            }

            if (gasCredito.Estado != Estados.PENDIENTE)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Solo puede Activar Créditos de Gasolina en Estado PENDIENTE"
                };
            }

            var ultimoCredito = _unidadDeTrabajo.GasolineraCreditos.FirstOrDefault(g => g.EsCreditoActual == true && g.Estado == Estados.ACTIVO);            
            gasCredito.ActivarGasCredito(ultimoCredito);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActivacionGasolineraCreditos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheGasolineraCreditos();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutGasolineraCreditos request)
        {
            var gasCredito = _unidadDeTrabajo.GasolineraCreditos.Find(request.GasolineraCredito.GasCreditoId);

            if (gasCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasCreditoId NO Existe"
                };
            }

            gasCredito.ActualizarGasCredito(request.GasolineraCredito.Credito, request.GasolineraCredito.FechaInicio);

            var mensajesValidacion = gasCredito.GetValidationErrors().ToList();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizarGasolineraCreditos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheGasolineraCreditos();

            return new ActualizarResponseDTO();
        }

        public EliminarResponseDTO Delete(DeleteGasolineraCredito request)
        {
            var gasCredito = _unidadDeTrabajo.GasolineraCreditos.Find(request.GasolineraCreditoId);

            if (gasCredito == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "GasCreditoId NO Existe"
                };
            }

            var mensajesValidacion = gasCredito.GetValidationErrorsDelete().ToList();
            if (mensajesValidacion.Any())
            {
                return new EliminarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.GasolineraCreditos.Remove(gasCredito);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "EliminarGasCredito");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheGasolineraCreditos();

            return new EliminarResponseDTO();
        }

        private void RemoverCacheGasolineraCreditos()
        {
            var listaKey = new List<string>
            {
                KeyCache.GasCreditos,
                KeyCache.Gasolineras,
                KeyCache.OrdenesCombustible
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
