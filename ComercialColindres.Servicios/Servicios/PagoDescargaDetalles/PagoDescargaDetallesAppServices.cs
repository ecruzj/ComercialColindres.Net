using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class PagoDescargaDetallesAppServices : IPagoDescargaDetallesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public PagoDescargaDetallesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                               ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }

        public List<PagoDescargaDetallesDTO> Get(GetPagoDescargasDetallePorPagoDescargaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.PagoDescargasDetalle, "GetPagoDescargasDetallePorPagoDescargaId", request.PagoDescargasId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<PagoDescargaDetalles> datos = _unidadDeTrabajo.PagoDescargaDetalles.Where(pd => pd.PagoDescargaId == request.PagoDescargasId)
                                                                    .OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<PagoDescargaDetalles>, IEnumerable<PagoDescargaDetallesDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostPagoDescargasDetalle request)
        {
            var pagoDescarga = _unidadDeTrabajo.PagoDescargadores.Find(request.PagoDescargaId);

            if (pagoDescarga == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PagoDescargaId no existe"
                };
            }

            var creditosRequest = request.PagoDescargaDetalle.Select(l => l.LineaCreditoId).Distinct();
            var listaLineasCredito = ObtenerLineasCredito(creditosRequest);
            var listaPagosDescargaDetalle = pagoDescarga.PagoDescargaDetalles.ToList();

            //Verificar si removieron Items
            var removerPagoDescargaDetalles = RemoverPagoDescargaDetalles(pagoDescarga, request.PagoDescargaDetalle);

            if (!string.IsNullOrWhiteSpace(removerPagoDescargaDetalles))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = removerPagoDescargaDetalles
                };
            }

            IEnumerable<string> mensajesValidacion;

            foreach (var itemPagoDetalle in request.PagoDescargaDetalle)
            {
                var pagoDetalle = listaPagosDescargaDetalle
                                     .FirstOrDefault(p => p.PagoDescargaDetalleId == itemPagoDetalle.PagoDescargaDetalleId);

                var lineaCreditoCierre = listaLineasCredito.FirstOrDefault(c => c.LineaCreditoId == itemPagoDetalle.LineaCreditoId);

                if (lineaCreditoCierre == null)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = string.Format("No existe la Linea de Crédito {0}", itemPagoDetalle.CodigoLineaCredito)
                    };
                }

                //Hubo una Actualizacion
                if (pagoDetalle != null && itemPagoDetalle.EstaEditandoRegistro)
                {
                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == itemPagoDetalle.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, itemPagoDetalle.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    pagoDetalle.ActualizarPagoDescargaDetalle(pagoDetalle.FormaDePago, pagoDetalle.LineaCreditoId, pagoDetalle.NoDocumento, pagoDetalle.Monto);
                    mensajesValidacion = pagoDetalle.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }

                if (pagoDetalle == null)
                {
                    //Se agrego un nuevo pago
                    var nuevoPagoDescargaDetalle = new PagoDescargaDetalles(request.PagoDescargaId, itemPagoDetalle.FormaDePago, itemPagoDetalle.LineaCreditoId, 
                                                               itemPagoDetalle.NoDocumento, itemPagoDetalle.Monto);
                    mensajesValidacion = nuevoPagoDescargaDetalle.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == itemPagoDetalle.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, itemPagoDetalle.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    _unidadDeTrabajo.PagoDescargaDetalles.Add(nuevoPagoDescargaDetalle);
                }
            }

            var montoJustificado = pagoDescarga.PagoDescargaDetalles.Sum(c => c.Monto);
            var totalAPagar = pagoDescarga.ObtenerTotalPago();

            if (montoJustificado > totalAPagar)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("La Justificación de Pago L. {0} supera el total a Pagar de las Descargas L. {1}", montoJustificado, totalAPagar)
                };
            }

            pagoDescarga.ActualizarEstadoPagoDescarga();
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "JustificacionPagoDescargas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
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

        private string RemoverPagoDescargaDetalles(PagoDescargadores pagoDescargadores, List<PagoDescargaDetallesDTO> pagoDescargaDetallesRequest)
        {
            var pagoDescargaDetalles = pagoDescargadores.PagoDescargaDetalles.ToList();

            foreach (var pagoDescargaDetalle in pagoDescargaDetalles)
            {
                var pagoDetalle = pagoDescargaDetallesRequest
                                  .FirstOrDefault(p => p.PagoDescargaDetalleId == pagoDescargaDetalle.PagoDescargaDetalleId);

                if (pagoDetalle == null)
                {
                    if (_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(pagoDescargaDetalle.LineaCredito))
                    {
                        pagoDescargadores.PagoDescargaDetalles.Remove(pagoDescargaDetalle);
                    }
                    else
                    {
                        return string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                     pagoDescargaDetalle.LineaCredito.CodigoLineaCredito, pagoDescargaDetalle.LineaCredito.Estado);
                    }
                }

            }

            return string.Empty;
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.Descargadores,
                KeyCache.PagoDescargas,
                KeyCache.PagoDescargasDetalle,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones,
                KeyCache.Boletas
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
