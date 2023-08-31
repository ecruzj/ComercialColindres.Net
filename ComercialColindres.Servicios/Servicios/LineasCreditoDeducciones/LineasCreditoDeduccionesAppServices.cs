using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.DTOs.ResponseDTOs;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;

namespace ComercialColindres.Servicios.Servicios
{
    public class LineasCreditoDeduccionesAppServices : ILineasCreditoDeduccionesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;

        public LineasCreditoDeduccionesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                                   ILineasCreditoDeduccionesDomainServices lineasCreditoDeduccionesDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _lineasCreditoDeduccionesDomainServices = lineasCreditoDeduccionesDomainServices;
        }        

        public List<LineasCreditoDeduccionesDTO> Get(GetDeduccionesOperativosPorLineaCreditoId request)
        {
            return ArmarDeduccionesOperativas(request.LineaCreditoId);
        }

        private List<LineasCreditoDeduccionesDTO> ArmarDeduccionesOperativas(int lineaCreditoId)
        {
            var lineaCredito = _unidadDeTrabajo.LineasCredito.Find(lineaCreditoId);
            var listaCreditoDeducciones = new List<LineasCreditoDeduccionesDTO>();
            
            if (lineaCredito == null)
            {
                return listaCreditoDeducciones;
            }

            var cierreBoletas = lineaCredito.BoletaCierres;

            foreach (var cierreBoleta in cierreBoletas)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = cierreBoleta.Monto,
                    NoDocumento = cierreBoleta.NoDocumento,
                    FormaDePago = cierreBoleta.FormaDePago,
                    Descripcion = string.Format("Pago de Boleta {0}", cierreBoleta.Boleta.CodigoBoleta),
                    FechaTransaccion = cierreBoleta.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            var prestamosTransferencias = lineaCredito.PrestamoTransferencias;

            foreach (var prestamoTransferencia in prestamosTransferencias)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = prestamoTransferencia.Monto,
                    NoDocumento = prestamoTransferencia.NoDocumento,
                    FormaDePago = prestamoTransferencia.FormaDePago,
                    Descripcion = string.Format("Transferencia del Prestamo {0}", prestamoTransferencia.Prestamo.CodigoPrestamo),
                    FechaTransaccion = prestamoTransferencia.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            var pagoDescargadores = lineaCredito.PagoDescargaDetalles;

            foreach (var pagoDescarga in pagoDescargadores)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = pagoDescarga.Monto,
                    NoDocumento = pagoDescarga.NoDocumento,
                    FormaDePago = pagoDescarga.FormaDePago,
                    Descripcion = string.Format("Pago de Descargadores {0}", pagoDescarga.PagoDescargador.CodigoPagoDescarga),
                    FechaTransaccion = pagoDescarga.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            var gasCreditoPagos = lineaCredito.GasolineraCreditoPagos;

            foreach (var pagoGasCredito in gasCreditoPagos)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = pagoGasCredito.Monto,
                    NoDocumento = pagoGasCredito.NoDocumento,
                    FormaDePago = pagoGasCredito.FormaDePago,
                    Descripcion = string.Format("Pago de Gas Crédito {0}", pagoGasCredito.GasolineraCredito.CodigoGasCredito),
                    FechaTransaccion = pagoGasCredito.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            var ajusteBoletas = lineaCredito.AjusteBoletaDetalles;

            foreach (AjusteBoletaDetalle ajusteBoleta in ajusteBoletas)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = ajusteBoleta.Monto,
                    NoDocumento = ajusteBoleta.NoDocumento,
                    FormaDePago = "Deposito",
                    Descripcion = $"Ajuste #Boleta {ajusteBoleta.AjusteBoleta.Boleta.CodigoBoleta}",
                    FechaTransaccion = ajusteBoleta.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            var boletasOtrasDeducciones = lineaCredito.BoletaOtrasDeducciones;
            foreach (var boletaOtraDeduccion in boletasOtrasDeducciones)
            {
                var creditoDeduccion = new LineasCreditoDeduccionesDTO
                {
                    Monto = boletaOtraDeduccion.Monto,
                    NoDocumento = boletaOtraDeduccion.NoDocumento,
                    FormaDePago = boletaOtraDeduccion.FormaDePago,
                    Descripcion = string.Format("{0} de la Boleta {1}", boletaOtraDeduccion.MotivoDeduccion, boletaOtraDeduccion.Boleta.CodigoBoleta),
                    FechaTransaccion = boletaOtraDeduccion.FechaTransaccion
                };

                listaCreditoDeducciones.Add(creditoDeduccion);
            }

            return listaCreditoDeducciones.OrderByDescending(f => f.FechaTransaccion).ToList();
        }

        public List<LineasCreditoDeduccionesDTO> Get(GetDeduccionesVariasPorLineaCreditoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.LineasCreditoDeducciones, "GetDeduccionesVariasPorLineaCreditoId", request.LineaCreditoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<LineasCreditoDeducciones> datos = _unidadDeTrabajo.LineasCreditoDeducciones
                                                            .Where(d => d.LineaCreditoId == request.LineaCreditoId && d.EsGastoOperativo == false)
                                                            .OrderByDescending(o => o.FechaTransaccion).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<LineasCreditoDeducciones>, IEnumerable<LineasCreditoDeduccionesDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostDeduccionVarios request)
        {
            var deduccionRequest = request.LineaCreditoDeduccion;
            LineasCreditoDeducciones lineaCreditoDeduccion = _unidadDeTrabajo.LineasCreditoDeducciones.FirstOrDefault(d => d.Descripcion == deduccionRequest.Descripcion 
                                                                                                                      && d.Monto == deduccionRequest.Monto 
                                                                                                                      && d.NoDocumento == deduccionRequest.NoDocumento);


            if (lineaCreditoDeduccion != null && (lineaCreditoDeduccion.FechaCreacion.Date == deduccionRequest.FechaCreacion.Date))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Deducción YA existe!"
                };
            }

            var lineaCredito = _unidadDeTrabajo.LineasCredito.FirstOrDefault(d => d.LineaCreditoId == request.LineaCreditoDeduccion.LineaCreditoId);
            var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(lineaCredito, deduccionRequest.Monto);

            if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = aplicarDeduccion
                };
            }

            lineaCreditoDeduccion = new LineasCreditoDeducciones(lineaCredito.LineaCreditoId, deduccionRequest.Descripcion, deduccionRequest.Monto, false,
                                                                 deduccionRequest.NoDocumento, deduccionRequest.FechaCreacion, lineaCredito.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco);

            var mensajesValidacion = lineaCreditoDeduccion.GetValidationErrors();

            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.LineasCreditoDeducciones.Add(lineaCreditoDeduccion);            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CrearLineaCreditoDeduccion");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutDeduccionVarios request)
        {
            var deduccion = _unidadDeTrabajo.LineasCreditoDeducciones
                            .FirstOrDefault(d => d.LineaCreditoDeduccionId == request.LineaCreditoDeduccion.LineaCreditoDeduccionId);

            if (deduccion.LineasCredito.Estado != Estados.ACTIVO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "El estado de la Linea de Crédito debe ser ACTIVO"
                };
            }

            var aplicarDeduccion = _lineasCreditoDeduccionesDomainServices.AplicarDeduccionCredito(deduccion.LineasCredito, deduccion.Monto);

            if (!string.IsNullOrWhiteSpace(aplicarDeduccion))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = aplicarDeduccion
                };
            }

            deduccion.ActualizarDeduccionLineaCredito(request.LineaCreditoDeduccion.Descripcion, request.LineaCreditoDeduccion.Monto, 
                                                      request.LineaCreditoDeduccion.NoDocumento, request.LineaCreditoDeduccion.FechaCreacion,
                                                      deduccion.LineasCredito.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco);

            var mensajesValidacion = deduccion.GetValidationErrors();

            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "UpdatedLineaCreditoDeduccion");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new ActualizarResponseDTO();
        }

        public EliminarResponseDTO Delete(DeleteDeduccionVarios request)
        {
            var deduccion = _unidadDeTrabajo.LineasCreditoDeducciones
                            .FirstOrDefault(d => d.LineaCreditoDeduccionId == request.LineaCreditoDeuccionId);

            if (deduccion.LineasCredito.Estado != Estados.ACTIVO)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "El estado de la Linea de Crédito debe ser ACTIVO"
                };
            }

            _unidadDeTrabajo.LineasCreditoDeducciones.Remove(deduccion);            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "DeletedLineaCreditoDeduccion");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCache();

            return new EliminarResponseDTO();
        }

        private void RemoverCache()
        {
            var listaKey = new List<string>
            {
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
