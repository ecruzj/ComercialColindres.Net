using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Helpers;
using ComercialColindres.Datos.Recursos;
using System;
using ServidorCore.EntornoDatos;
using ComercialColindres.Datos.Especificaciones;

namespace ComercialColindres.Servicios.Servicios
{
    public class LineasCreditoAppServices : ILineasCreditoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public LineasCreditoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        private const string _codigoCorrelativo_LineaCredito = "LC";

        public List<LineasCreditoDTO> Get(GetLineasCreditoPorBancoPorTipoCuenta request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}{4}{5}", KeyCache.LineasCredito, "GetLineasCreditoPorBancoId", request.BancoId, 
                                                               request.SucursalId, request.UsuarioId, request.CuentaFinancieraTipoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuario = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
                var especificacion = LineasCreditoEspecificaciones.FiltrarLineasCreditoPorBancoPorTipoCuenta(request.SucursalId, usuario, request.BancoId, request.CuentaFinancieraTipoId);
                List<LineasCredito> datos = _unidadDeTrabajo.LineasCredito.Where(especificacion.EvalFunc).ToList();
                
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<LineasCredito>, IEnumerable<LineasCreditoDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<LineasCreditoDTO> Get(GetLineasCreditoCajaChica request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.LineasCredito, "GetLineasCreditoCajaChica", request.SucursalId, request.UsuarioId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var usuario = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
                var tienePermisoAdministrativo = usuario != null ? usuario.TienePermisoAdministrativo() : false;

                List<LineasCredito> datos = _unidadDeTrabajo.LineasCredito
                                                            .Where(l => l.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco == false
                                                                   && l.EsLineaCreditoActual == true
                                                                   && (l.CuentasFinanciera.EsCuentaAdministrativa == tienePermisoAdministrativo
                                                                       || l.CuentasFinanciera.EsCuentaAdministrativa == false))
                                                            .OrderByDescending(o => o.CodigoLineaCredito).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<LineasCredito>, IEnumerable<LineasCreditoDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public BusquedaLineasCreditoDTO Get(GetByValorLineasCredito request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}-{4}", KeyCache.LineasCredito, request.Filtro, request.PaginaActual, request.CantidadRegistros, request.SucursalId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var usuario = _unidadDeTrabajo.Usuarios.Find(request.UsuarioId);
                var especificacion = LineasCreditoEspecificaciones.FiltrarLineasCreditoBusqueda(request.Filtro, request.SucursalId, usuario);
                List<LineasCredito> datos = _unidadDeTrabajo.LineasCredito.Where(especificacion.EvalFunc).OrderByDescending(o => o.CodigoLineaCredito).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<LineasCredito>, IEnumerable<LineasCreditoDTO>>(datosPaginados.Items as IEnumerable<LineasCredito>);

                var dto = new BusquedaLineasCreditoDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<LineasCreditoDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public LineasCreditoDTO Get(GetLineaCreditoUltimo request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.LineasCredito, "GetLineaCreditoUltimo", request.SucursalId, request.Fecha.Year.ToString());
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var prefijoControl = _codigoCorrelativo_LineaCredito + DateTime.Now.ToString("yy") + "-";
                var creditLineCode = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_LineaCredito, prefijoControl, false);
                return new LineasCreditoDTO { CodigoLineaCredito = creditLineCode };
            });
            return retorno;
        }

        public ActualizarResponseDTO Post(PostLineaCredito request)
        {
            var lineaCredito = _unidadDeTrabajo.LineasCredito.Find(request.LineaCredito.LineaCreditoId);

            if (lineaCredito != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La Linea de Crédito YA existe!"
                };
            }

            var cuentaFinanciera = _unidadDeTrabajo.CuentasFinancieras.Find(request.LineaCredito.CuentaFinancieraId);

            if (cuentaFinanciera == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "CuentaFinancieraId NO existe!"
                };
            }

            var datosRequest = request.LineaCredito;
            lineaCredito = new LineasCredito("???", request.SucursalId, datosRequest.CuentaFinancieraId, datosRequest.NoDocumento, datosRequest.Observaciones, datosRequest.FechaCreacion,
                                             datosRequest.MontoInicial, datosRequest.Saldo, datosRequest.CreadoPor);

            var mensajesValidacion = lineaCredito.GetValidationErrors().ToList();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            lineaCredito.CodigoLineaCredito = CorrelativosHelper.ObtenerCorrelativo(_unidadDeTrabajo, request.SucursalId, _codigoCorrelativo_LineaCredito, string.Empty, true);
            _unidadDeTrabajo.LineasCredito.Add(lineaCredito);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CreacionLineaCreditos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutLineaCreditoAnular request)
        {
            var lineaCredito = _unidadDeTrabajo.LineasCredito.Find(request.LineaCreditoId);

            if (lineaCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "LineaCreditoId NO Existe!"
                };
            }

            var validacionAnularCredito = lineaCredito.AnularLineaCredito();
            if (!string.IsNullOrWhiteSpace(validacionAnularCredito))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = validacionAnularCredito
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "AnularLineaCredito");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutLineaCredito request)
        {
            var lineaCredito = _unidadDeTrabajo.LineasCredito.Find(request.LineaCredito.LineaCreditoId);

            if (lineaCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "LineaCreditoId NO Existe!"
                };
            }

            var datosUpdate = request.LineaCredito;
            var validacionActualizacion = lineaCredito.ActualizarLineaCredito(datosUpdate.CuentaFinancieraId, datosUpdate.NoDocumento, datosUpdate.Observaciones, datosUpdate.FechaCreacion,
                                                                              datosUpdate.MontoInicial, datosUpdate.Saldo, datosUpdate.CreadoPor);

            if (!string.IsNullOrWhiteSpace(validacionActualizacion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = validacionActualizacion
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizarLineaCredito");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutActivarLineaCredito request)
        {
            var lineaCredito = _unidadDeTrabajo.LineasCredito.Find(request.LineaCreditoId);

            if (lineaCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "LineaCreditoId NO Existe!"
                };
            }

            if (lineaCredito.Estado != Estados.NUEVO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "LineaCreditoId debe ser NUEVO para Activarlo"
                };
            }

            var bancoId = lineaCredito.CuentasFinanciera.BancoId;
            var ultimaLineaCredito = _unidadDeTrabajo.LineasCredito.FirstOrDefault(lc => lc.EsLineaCreditoActual == true
                                                                                   && lc.Estado == Estados.ACTIVO
                                                                                   && 
                                                                                   (bancoId != null ? lc.CuentasFinanciera.BancoId == bancoId
                                                                                    && lc.CuentasFinanciera.CuentaNo == lineaCredito.CuentasFinanciera.CuentaNo
                                                                                   : !lc.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco));
            lineaCredito.ActivarLineaCredito(ultimaLineaCredito);
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActivacionLineaCreditos");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones,
                KeyCache.BoletasCierres,
                KeyCache.PagoDescargas,
                KeyCache.GasCreditoPagos,
                KeyCache.PagoPrestamos
            };
            _cacheAdapter.Remove(listaKey);
        }        
    }
}
