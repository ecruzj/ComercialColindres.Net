using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using System;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class PrestamosTransferenciasAppServices : IPrestamosTransferenciasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public PrestamosTransferenciasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                                  ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }

        public List<PrestamosTransferenciasDTO> Get(GetPrestamosTransferenciasPorPrestamoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.PrestamosTransferencias, "GetPrestamosTransferenciasPorPrestamoId", request.PrestamoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PrestamosTransferencias> datos = _unidadDeTrabajo.PrestamosTransferencias.Where(p => p.PrestamoId == request.PrestamoId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PrestamosTransferencias>, IEnumerable<PrestamosTransferenciasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostPrestamoTransferencias request)
        {
            var prestamo = _unidadDeTrabajo.Prestamos.Find(request.PrestamoId);

            if (prestamo == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PrestamoId NO existe"
                };
            }

            var creditosRequest = request.PrestamoTransferencias.Select(l => l.LineaCreditoId).Distinct();
            var listaLineasCredito = ObtenerLineasCredito(creditosRequest);
            var listaPrestamosTransferencias = prestamo.PrestamosTransferencias.ToList();

            //Verificar si removieron Items
            var removerPrestamosTransferencias = RemoverPrestamosTransferencias(prestamo, request.PrestamoTransferencias);
            
            if (!string.IsNullOrWhiteSpace(removerPrestamosTransferencias))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = removerPrestamosTransferencias
                };
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var transferencia in request.PrestamoTransferencias)
            {
                var transferenciaPrestamo = listaPrestamosTransferencias
                                     .FirstOrDefault(c => c.PrestamoTransferenciaId == transferencia.PrestamoTransferenciaId);

                var lineaCreditoCierre = listaLineasCredito.FirstOrDefault(c => c.LineaCreditoId == transferencia.LineaCreditoId);

                if (lineaCreditoCierre == null)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = string.Format("No existe la Linea de Crédito {0}", transferencia.CodigoLineaCredito)
                    };
                }

                //Hubo una Actualizacion
                if (transferenciaPrestamo != null && transferencia.EstaEditandoRegistro)
                {
                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == transferencia.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, transferencia.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    transferenciaPrestamo.ActualizarTransferenciaPrestamo(transferencia.FormaDePago, transferencia.LineaCreditoId, transferencia.NoDocumento, transferencia.Monto);
                    mensajesValidacion = transferenciaPrestamo.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                
                if (transferenciaPrestamo == null)
                {
                    //Se agrego una nueva cuenta
                    var nuevaTransferencia = new PrestamosTransferencias(request.PrestamoId, transferencia.FormaDePago, transferencia.LineaCreditoId, transferencia.NoDocumento, transferencia.Monto);
                    mensajesValidacion = nuevaTransferencia.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == transferencia.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, transferencia.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    _unidadDeTrabajo.PrestamosTransferencias.Add(nuevaTransferencia);
                }
            }

            var montoJustificado = prestamo.PrestamosTransferencias.Sum(c => c.Monto);

            if (montoJustificado > prestamo.MontoPrestamo)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("La Justificación de la Transferencia L. {0} supera el total del Prestamo L. {1}", montoJustificado, prestamo.MontoPrestamo)
                };
            }

            prestamo.ActualizarEstadoPrestamo();
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "JustificacionTransferencias");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        private string RemoverPrestamosTransferencias(Prestamos prestamo, List<PrestamosTransferenciasDTO> PrestamosTransferenciasRequest)
        {
            var prestamosTransferencias = prestamo.PrestamosTransferencias.ToList();

            foreach(var prestamoTransferencia in prestamosTransferencias)
            {
                var transferencia = PrestamosTransferenciasRequest
                                    .FirstOrDefault(t => t.PrestamoTransferenciaId == prestamoTransferencia.PrestamoTransferenciaId);

                if (transferencia == null)
                {
                    if (_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(prestamoTransferencia.LineaCredito))
                    {
                        prestamo.PrestamosTransferencias.Remove(prestamoTransferencia);
                    }
                    else
                    {
                        return string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                     prestamoTransferencia.LineaCredito.CodigoLineaCredito, prestamoTransferencia.LineaCredito.Estado);
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
                KeyCache.Prestamos,
                KeyCache.PrestamosTransferencias,
                KeyCache.BoletasCierres,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
