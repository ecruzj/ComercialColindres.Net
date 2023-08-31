using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using System;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletaCierresAppServices : IBoletaCierresAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;
        IBoletasDetalleDomainServices _boletasDetalleDomainServices;
        IPagoPrestamosDomainServices _pagoPrestamosDomainServices;
        IReportesAppServices _reportingServices;
        IBoletaCierreDomainServices _boletaCierreDomainServices;
        IBoletaDetallesAppServices _boletaDetallesAppServices;
        IBoletaHumedadDomainServices _boletaHumedadDomainServices;
        IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        public BoletaCierresAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                        ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices,
                                        IBoletasDetalleDomainServices boletasDetalleDomainServices,
                                        IPagoPrestamosDomainServices pagoPrestamosDomainServices,
                                        IReportesAppServices reportingServices,
                                        IBoletaCierreDomainServices boletaCierreDomainServices,
                                        IBoletaDetallesAppServices boletaDetallesAppServices,
                                        IBoletaHumedadDomainServices boletaHumedadDomainServices,
                                        IAjusteBoletaDomainServices ajusteBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
            _boletasDetalleDomainServices = boletasDetalleDomainServices;
            _pagoPrestamosDomainServices = pagoPrestamosDomainServices;
            _reportingServices = reportingServices;
            _boletaCierreDomainServices = boletaCierreDomainServices;
            _boletaDetallesAppServices = boletaDetallesAppServices;
            _boletaHumedadDomainServices = boletaHumedadDomainServices;
            _ajusteBoletaDomainServices = ajusteBoletaDomainServices;
        }

        public List<BoletaCierresDTO> Get(GetBoletasCierrePorBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.BoletasCierres, "GetBoletasCierrePorBoletaId", request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<BoletaCierres> datos = _unidadDeTrabajo.BoletaCierres.Where(b => b.BoletaId == request.BoletaId)
                                                                            .OrderByDescending(o => o.Monto).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<BoletaCierres>, IEnumerable<BoletaCierresDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }
        
        public ActualizarResponseDTO Post(PostBoletasCierre request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "BoletaId NO existe"
                };
            }

            if (boleta.Estado == Estados.CERRADO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La boleta ya está cerrada"
                };
            }

            var creditosRequest = request.BoletaCierres.Select(l => l.LineaCreditoId).Distinct();
            var listaLineasCredito = ObtenerLineasCredito(creditosRequest);
            var listaBoletaCierres = _unidadDeTrabajo.BoletaCierres.Where(b => b.BoletaId == boleta.BoletaId).ToList();

            var removerBoletaCierres = RemoverBoletaCierres(boleta, listaBoletaCierres, request.BoletaCierres);

            if (!string.IsNullOrWhiteSpace(removerBoletaCierres))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = removerBoletaCierres
                };
            }
            
            IEnumerable<string> mensajesValidacion;
            
            foreach (var cierre in request.BoletaCierres)
            {
                var cierreBoleta = listaBoletaCierres
                                     .FirstOrDefault(c => c.BoletaCierreId == cierre.BoletaCierreId);

                var lineaCreditoCierre = listaLineasCredito.FirstOrDefault(c => c.LineaCreditoId == cierre.LineaCreditoId);

                if (lineaCreditoCierre == null)
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = string.Format("No existe la Linea de Crédito {0}", cierre.CodigoLineaCredito)
                    };
                }

                //Hubo una Actualizacion
                if (cierreBoleta != null && cierre.EstaEditandoRegistro)
                {
                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == cierre.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, cierre.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    cierreBoleta.ActualizarBoletaCierre(cierre.FormaDePago, cierre.LineaCreditoId, cierre.NoDocumento, cierre.Monto, cierre.FechaPago);
                    mensajesValidacion = cierreBoleta.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }                    
                }
                
                if (cierreBoleta == null)
                {
                    //Se agrego un nuevo item
                    var nuevoDetalle = new BoletaCierres(request.BoletaId, cierre.FormaDePago, cierre.LineaCreditoId, cierre.NoDocumento, cierre.Monto, cierre.FechaPago);
                    mensajesValidacion = nuevoDetalle.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    var lineaCredito = listaLineasCredito.FirstOrDefault(l => l.LineaCreditoId == cierre.LineaCreditoId);
                    var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, cierre.Monto);

                    if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = aplicarDeduccion
                        };
                    }

                    boleta.AddBoletaCierre(nuevoDetalle);
                }
            }
            
            var validarEstadoBoleta = _boletasDetalleDomainServices.ActualizarEstadoBoleta(boleta);

            if (!string.IsNullOrWhiteSpace(validarEstadoBoleta))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = validarEstadoBoleta
                };
            }

            if (boleta.Estado == Estados.CERRADO)
            {
                //Registrar Boleta Detalles
                var validacionBoletaDetalles = RegistrarBoletaDeducciones(boleta, request.BoletaDetalles);

                if (!string.IsNullOrWhiteSpace(validacionBoletaDetalles))
                {
                    return new ActualizarResponseDTO
                    {
                        MensajeError = validacionBoletaDetalles
                    };
                }

                //En caso de que el prestamo este saldado, cerrarlo.
                _pagoPrestamosDomainServices.CloseLoan(boleta.PagoPrestamos);

                _boletaHumedadDomainServices.CloseBoletaHumidity(boleta.BoletasHumedadPagos.ToList());

                _ajusteBoletaDomainServices.TryCloseAjusteBoleta(boleta.AjusteBoletaPagos.ToList());
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "JustificacionPagoCierreBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public List<BoletaCierresDTO> CloseBoletaMasive(CloseBoletaMasive request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.RequestUserInfo == null) throw new ArgumentNullException(nameof(request.RequestUserInfo));
            if (request.BoletaCierres == null) throw new ArgumentNullException(nameof(request.BoletaCierres));

            List<BoletaCierresDTO> boletasCierres = new List<BoletaCierresDTO>();
            List<int> creditLinesId = request.BoletaCierres.Select(l => l.LineaCreditoId).Distinct().ToList();

            GetBoletaPaymentPending pendingPaymentRequest = new GetBoletaPaymentPending { ProveedorId = request.VendorId, IsPartialPayment = request.IsPartialPayment, BoletasId = request.BoletasToPay, RequestUserInfo = request.RequestUserInfo };
            RptBoletaPaymentPendingResumenDto pendingPaymentResult = _reportingServices.GetBoletaPaymentPending(pendingPaymentRequest);
            pendingPaymentResult.GetValuesForPayment();

            decimal paymentTotal = request.BoletaCierres.Sum(c => c.Monto);            

            if (pendingPaymentResult.SaldoPendiente != paymentTotal)
            {
                boletasCierres.Add(CreateException("", "El total del abono es diferente al total de pago pendiente!"));
                return boletasCierres;
            }

            List<int> boletasPending = pendingPaymentResult.BoletasPending.Select(b => b.BoletaId).Distinct().ToList();
            List<LineasCredito> creditLines = ObtenerLineasCredito(creditLinesId);
            List<Boletas> boletasOpen = _unidadDeTrabajo.Boletas.Where(b => boletasPending.Contains(b.BoletaId)).ToList();

            string validationMessageString = string.Empty;

            foreach (Boletas boletaPending in boletasOpen)
            {
                if (boletaPending.Estado == Estados.CERRADO)
                {
                    ValidateException("La Boleta está CERRADA!", boletaPending.CodigoBoleta, boletasCierres);
                    continue;
                }

                GetBoletasDetallePorBoletaId detailDeductionsRequest = new GetBoletasDetallePorBoletaId { BoletaId = boletaPending.BoletaId };
                List<BoletaDetallesDTO> detailDeductions = _boletaDetallesAppServices.Get(detailDeductionsRequest);

                foreach (BoletaCierresDTO itemClose in request.BoletaCierres)
                {
                    decimal averagePayment = _boletaCierreDomainServices.BuildAveragePaymentMasive(pendingPaymentResult.SaldoPendiente, itemClose.Monto, boletaPending);
                    BoletaCierres newCloseItem = new BoletaCierres(boletaPending.BoletaId, itemClose.FormaDePago, itemClose.LineaCreditoId, itemClose.NoDocumento, averagePayment, itemClose.FechaPago);
                    validationMessageString = newCloseItem.GetValidationErrors().FirstOrDefault();

                    if (!ValidateException(validationMessageString, boletaPending.CodigoBoleta, boletasCierres)) continue;

                    LineasCredito creditLine = creditLines.FirstOrDefault(l => l.LineaCreditoId == itemClose.LineaCreditoId);
                    validationMessageString = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(creditLine, averagePayment);

                    if (!ValidateException(validationMessageString, boletaPending.CodigoBoleta, boletasCierres)) continue;

                    boletaPending.AddItemBoletaClose(newCloseItem);
                }

                boletaPending.CloseBoleta();

                ////Registrar Boleta Detalles
                validationMessageString = RegistrarBoletaDeducciones(boletaPending, detailDeductions);

                if (!ValidateException(validationMessageString, boletaPending.CodigoBoleta, boletasCierres))
                {
                    boletaPending.DeleteWrongClose();
                    continue;
                }

                //En caso de que el prestamo este saldado, cerrarlo.
                _pagoPrestamosDomainServices.CloseLoan(boletaPending.PagoPrestamos);

                _boletaHumedadDomainServices.CloseBoletaHumidity(boletaPending.BoletasHumedadPagos.ToList());

                _ajusteBoletaDomainServices.TryCloseAjusteBoleta(boletaPending.AjusteBoletaPagos.ToList());
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "CloseBoletasMasive");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return boletasCierres;
        }
        
        private bool ValidateException(string validationMessageString, string codigoBoleta, List<BoletaCierresDTO> boletasCierres)
        {
            if (string.IsNullOrWhiteSpace(validationMessageString)) return true;

            boletasCierres.Add(CreateException(codigoBoleta, validationMessageString));
            return false;
        }

        private BoletaCierresDTO CreateException(string codigoBoleta, string errorValidation)
        {
            return new BoletaCierresDTO
            {
                CodigoBoleta = codigoBoleta,
                MensajeError = errorValidation
            };
        }

        private string RegistrarBoletaDeducciones(Boletas boleta, List<BoletaDetallesDTO> requestBoletaDetalles)
        {
            var datos = from reg in requestBoletaDetalles
                        select new BoletaDetalles
                        {
                            CodigoBoleta = boleta.BoletaId,
                            DeduccionId = reg.DeduccionId,
                            MontoDeduccion = reg.MontoDeduccion,
                            NoDocumento = reg.NoDocumento,
                            Observaciones = reg.Observaciones
                        };

            var listaDeducciones = _unidadDeTrabajo.Deducciones.ToList();           
        
           return _boletasDetalleDomainServices.RegistrarBoletaDeducciones(boleta, listaDeducciones);
        }

        private string RemoverBoletaCierres(Boletas boleta, List<BoletaCierres> listaBoletasCierres, List<BoletaCierresDTO> boletaCierresRequest)
        {
            foreach (var cierre in listaBoletasCierres)
            {
                var cierreBoleta = boletaCierresRequest
                                   .FirstOrDefault(c => c.BoletaCierreId == cierre.BoletaCierreId);

                if (cierreBoleta == null)
                {
                    if (_lineasCreditoDeduccionesDomainServices.AplicaRemoverDeduccionCredito(cierre.LineasCredito))
                    {
                        boleta.BoletaCierres.Remove(cierre);
                    }
                    else
                    {
                        return string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}",
                                                        cierre.LineasCredito.CodigoLineaCredito, cierre.LineasCredito.Estado);
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
                KeyCache.BoletasCierres,
                KeyCache.Boletas,
                KeyCache.OrdenesCombustible,
                KeyCache.GasCreditos,
                KeyCache.Prestamos,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones,
                KeyCache.AjusteBoletas,
                KeyCache.AjusteBoletaDetalles,
                KeyCache.AjusteBoletaPagos
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
