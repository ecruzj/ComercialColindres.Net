using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using System;

namespace ComercialColindres.Servicios.Servicios
{
    public class GasolineraCreditoPagosAppServices : IGasolineraCreditoPagosAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public GasolineraCreditoPagosAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                                 ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }

        public List<GasolineraCreditoPagosDTO> Get(GetGasCreditoPagosPorGasCreditoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.GasCreditoPagos, "GetGasCreditoPagosPorGasCreditoId", request.GasCreditoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<GasolineraCreditoPagos> datos = _unidadDeTrabajo.GasolineraCreditoPagos.Where(p => p.GasCreditoId == request.GasCreditoId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<GasolineraCreditoPagos>, IEnumerable<GasolineraCreditoPagosDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostGasCreditoPagos request)
        {
            var gasCredito = _unidadDeTrabajo.GasolineraCreditos.Find(request.GasCreditoId);

            if (gasCredito == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "GasCreditoId NO existe"
                };
            }

            var creditosRequest = request.GasolineraCreditoPagos.Select(l => l.LineaCreditoId).Distinct();
            var listaLineasCredito = ObtenerLineasCredito(creditosRequest);
            var listaGasCreditoPagos = gasCredito.GasolineraCreditoPagos.ToList();

            //Verificar si removieron Items
            var removerPagoGasCredito = RemoverPagoGasCreditos(gasCredito, request.GasolineraCreditoPagos);
            
            IEnumerable<string> mensajesValidacion;

            foreach (var pago in request.GasolineraCreditoPagos)
            {
                var gasPago = listaGasCreditoPagos
                                     .FirstOrDefault(p => p.GasCreditoPagoId == pago.GasCreditoPagoId);

                var lineaCreditoGas = listaLineasCredito.FirstOrDefault(c => c.LineaCreditoId == pago.LineaCreditoId);

                if (lineaCreditoGas == null)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = string.Format("No existe la Linea de Crédito {0}", pago.CodigoLineaCredito)
                    };
                }

                //Hubo una Actualizacion
                if (gasPago != null && pago.EstaEditandoRegistro)
                {
                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == pago.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, pago.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    gasPago.ActualizarCreditoPago(pago.FormaDePago, pago.LineaCreditoId, pago.NoDocumento, pago.Monto);
                    mensajesValidacion = gasPago.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                
                if (gasPago == null)
                {
                    //Se agrego un nuevo item
                    var nuevoGasPago = new GasolineraCreditoPagos(request.GasCreditoId, pago.FormaDePago, pago.LineaCreditoId, pago.NoDocumento, pago.Monto);
                    mensajesValidacion = nuevoGasPago.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == pago.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, pago.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    _unidadDeTrabajo.GasolineraCreditoPagos.Add(nuevoGasPago);
                }
            }

            var montoJustificado = gasCredito.GasolineraCreditoPagos.Sum(c => c.Monto);
            var totalAPagar = gasCredito.Credito;

            if (montoJustificado > totalAPagar)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("La Justificación de Pago L. {0} supera el total a Pagar del Crédito L. {1}", montoJustificado, totalAPagar)
                };
            }

            gasCredito.ActualizarEstadoCredito();

            if (gasCredito.Estado == Estados.ACTIVO)
            {
                var ultimoCredito = _unidadDeTrabajo.GasolineraCreditos.FirstOrDefault(g => g.EsCreditoActual == true && g.Estado == Estados.ACTIVO);
                gasCredito.ActivarGasCredito(ultimoCredito);              
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "JustificacionPagoGasCredito");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        private string RemoverPagoGasCreditos(GasolineraCreditos gasCredito, List<GasolineraCreditoPagosDTO> gasolineraCreditoPagosRequest)
        {
            var gasolineraCreditoPagos = gasCredito.GasolineraCreditoPagos.ToList();

            foreach (var gasCreditoPago in gasolineraCreditoPagos)
            {
                var gasPago = gasolineraCreditoPagosRequest
                              .FirstOrDefault(p => p.GasCreditoPagoId == gasCreditoPago.GasCreditoPagoId);

                if (gasPago == null)
                {
                    if (_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(gasCreditoPago.LineaCredito))
                    {
                        gasCredito.GasolineraCreditoPagos.Remove(gasCreditoPago);
                    }
                    else
                    {
                        return string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                     gasCreditoPago.LineaCredito.CodigoLineaCredito, gasCreditoPago.LineaCredito.Estado);
                    }
                }
            }
            return string.Empty;
        }

        private List<LineasCredito> ObtenerLineasCredito(IEnumerable<int> lineasCreditoRequest)
        {
            var lineasCredito = new List<LineasCredito>();

            foreach (var linea in lineasCreditoRequest)
            {
                var credito = _unidadDeTrabajo.LineasCredito.FirstOrDefault(lc => lc.LineaCreditoId == linea);

                if (credito != null)
                {
                    lineasCredito.Add(credito);
                }
            }

            return lineasCredito;
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.GasCreditoPagos,
                KeyCache.GasCreditos,
                KeyCache.OrdenesCombustible,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones,
                KeyCache.Boletas,
                KeyCache.BoletasCierres
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
