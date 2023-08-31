using System.Collections.Generic;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using System.Linq;
using ComercialColindres.Datos.Recursos;
using ServidorCore.Aplicacion;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Datos.Especificaciones;
using ServidorCore.EntornoDatos;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

namespace ComercialColindres.Servicios.Servicios
{
    public class OrdenesCombustibleAppServices : IOrdenesCombustibleAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletasDetalleDomainServices _boletasDetalleDomainSerives;
        IGasolineraCreditoDomainServices _gasolineraCreditoDomainServices;
        IOrdenCombustibleDomainServices _ordenCombustibleDomainServices;

        public OrdenesCombustibleAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter,
                                             IBoletasDetalleDomainServices boletasDetalleDomainSerives,
                                             IGasolineraCreditoDomainServices gasolineraCreditoDomainServices,
                                             IOrdenCombustibleDomainServices ordenCombustibleDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _boletasDetalleDomainSerives = boletasDetalleDomainSerives;
            _gasolineraCreditoDomainServices = gasolineraCreditoDomainServices;
            _ordenCombustibleDomainServices = ordenCombustibleDomainServices;
        }

        public BusquedaOrdenesCombustibleDTO Get(GetByValorOrdenesCombustible request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.OrdenesCombustible, request.Filtro, request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {

                var especificacion = OrdenesCombustibleEspecificaciones.FiltrarOrdenCombustibleBusqueda(request.Filtro);
                List<OrdenesCombustible> datos = _unidadDeTrabajo.OrdenesCombustible.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCombustible>, IEnumerable<OrdenesCombustibleDTO>>(datosPaginados.Items as IEnumerable<OrdenesCombustible>);

                var dto = new BusquedaOrdenesCombustibleDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<OrdenesCombustibleDTO>(datosDTO)
                };

                return dto;
            });
            return datosConsulta;
        }

        public List<OrdenesCombustibleDTO> Get(GetOrdenesCombustibleByBoletaId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.OrdenesCombustible, nameof(GetOrdenesCombustibleByBoletaId), request.BoletaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<OrdenesCombustible> datos = _unidadDeTrabajo.OrdenesCombustible.Where(c => c.BoletaId == request.BoletaId)
                                                                                           .OrderByDescending(o => o.FechaCreacion).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCombustible>, IEnumerable<OrdenesCombustibleDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<OrdenesCombustibleDTO> Get(GetOrderFuelByVendorId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.OrdenesCombustible, nameof(GetOrderFuelByVendorId), request.ProveedorId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<OrdenesCombustible> datos = _unidadDeTrabajo.OrdenesCombustible.Where(c => c.ProveedorId == request.ProveedorId 
                                                                                           && c.BoletaId == null).OrderBy(o => o.FechaCreacion).ToList();
                datos = datos.Where(f => !f.HasFuelManualPayments()).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCombustible>, IEnumerable<OrdenesCombustibleDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public List<OrdenesCombustibleDTO> Get(GetOrdenesCombustiblePorGasCreditoId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.OrdenesCombustible, "GetOrdenesCombustiblePorGasCreditoId", request.GasCreditoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<OrdenesCombustible> datos = _unidadDeTrabajo.OrdenesCombustible.Where(c => c.GasCreditoId == request.GasCreditoId)
                                                                                    .OrderByDescending(o => o.FechaCreacion).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCombustible>, IEnumerable<OrdenesCombustibleDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public OrdenesCombustibleDTO Post(PostOrdenesCombustible request)
        {
            var ordenCombustible = _unidadDeTrabajo.OrdenesCombustible.Find(request.OrdenCombustible.OrdenCombustibleId);
            var orderRequest = request.OrdenCombustible;

            if (ordenCombustible != null)
            {
                return new OrdenesCombustibleDTO
                {
                    MensajeError = "OrdenCombustibleId YA existe"
                };
            }

            var validacionCodigoFactura = VerificarCodigoFacturas(request.OrdenCombustible.CodigoFactura, request.OrdenCombustible.GasCreditoId, false);

            if (!string.IsNullOrWhiteSpace(validacionCodigoFactura))
            {
                return new OrdenesCombustibleDTO
                {
                    MensajeError = validacionCodigoFactura
                };
            }

            var gasCredito = _unidadDeTrabajo.GasolineraCreditos.Find(orderRequest.GasCreditoId);


            if (!_gasolineraCreditoDomainServices.PuedeUtilizarGasCredito(gasCredito, out string mensajeValidacion))
            {
                return new OrdenesCombustibleDTO { MensajeError = mensajeValidacion };
            }

            ordenCombustible = new OrdenesCombustible(orderRequest.CodigoFactura, orderRequest.GasCreditoId, orderRequest.AutorizadoPor,
                                                      orderRequest.PlacaEquipo, orderRequest.EsOrdenPersonal, orderRequest.Monto, orderRequest.Observaciones, 
                                                      orderRequest.FechaCreacion, orderRequest.Imagen, orderRequest.ProveedorId);

            var mensajesValidacion = ordenCombustible.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new OrdenesCombustibleDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            _unidadDeTrabajo.OrdenesCombustible.Add(ordenCombustible);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "AssignOrderCombustible");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCombustible();

            return new OrdenesCombustibleDTO();
        }

        public ActualizarResponseDTO Put(PutOrdenesCombustible request)
        {
            var ordenCombustible = _unidadDeTrabajo.OrdenesCombustible.Find(request.OrdenCombustible.OrdenCombustibleId);
            var fuelOrderRequest = request.OrdenCombustible;

            if (ordenCombustible == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "OrdenCombbustibleId NO Existe"
                };
            }

            if (!_ordenCombustibleDomainServices.PuedeActualizarOrdenCombustible(ordenCombustible, fuelOrderRequest.Monto, out string mensajeValidacion))
            {
                return new ActualizarResponseDTO { MensajeError = mensajeValidacion };
            }

            var existeDocumentoDuplicado = ExistenDocumentosOrdenCombustible(fuelOrderRequest.CodigoFactura, ordenCombustible.GasolineraCredito.Gasolinera.GasolineraId);

            if (existeDocumentoDuplicado)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("El Código de Factura {0} ya existe en otra Orden de Combustible", fuelOrderRequest.CodigoFactura)
                };
            }    
            
            ordenCombustible.ActualizarOrdenCombustible(fuelOrderRequest.CodigoFactura, fuelOrderRequest.EsOrdenPersonal, fuelOrderRequest.AutorizadoPor,
                                                      fuelOrderRequest.BoletaId, fuelOrderRequest.ProveedorId, fuelOrderRequest.PlacaEquipo, fuelOrderRequest.Monto,
                                                      fuelOrderRequest.Observaciones, fuelOrderRequest.FechaCreacion, fuelOrderRequest.Imagen);

            var mensajesValidacion = ordenCombustible.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizacionOrdenCombustible");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCombustible();

            return new ActualizarResponseDTO();
        }

        public OrdenesCombustibleDTO AssignFuelOrdersToBoleta(PutOrdenesCombustibleABoleta request)
        {
            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new OrdenesCombustibleDTO { ValidationErrorMessage = "BoletaId no Existe!" };
            }

            if (boleta.Estado != Estados.ACTIVO)
            {
                return new OrdenesCombustibleDTO { ValidationErrorMessage = "El estado de la Boleta debe ser ACTIVO" };
            }

            if (!RemoveOldFuelOrders(boleta, request.OrdenesCombustible, out string errorMessage))
            {
                return new OrdenesCombustibleDTO { ValidationErrorMessage = errorMessage };
            }

            List<OrdenesCombustible> currentFuelOrders = boleta.OrdenesCombustible.ToList();
            IEnumerable<int> fuelOrdersId = request.OrdenesCombustible.Select(f => f.OrdenCombustibleId).Distinct();
            List<OrdenesCombustible> fuelOrders = _unidadDeTrabajo.OrdenesCombustible.Where(f => fuelOrdersId.Contains(f.OrdenCombustibleId)).ToList();
            List<OrdenesCombustible> fuelOrdersPending = _unidadDeTrabajo.OrdenesCombustible.Where(p => p.ProveedorId == boleta.ProveedorId && p.BoletaId == null).ToList();

            foreach (int fuelOrderId in fuelOrdersId)
            {
                OrdenesCombustible fuelOrder = fuelOrders.FirstOrDefault(f => f.OrdenCombustibleId == fuelOrderId);

                if (fuelOrder == null)
                {
                    return new OrdenesCombustibleDTO { ValidationErrorMessage = "Existe una OrdenCombustibleId inválido" };
                }
            }

            foreach (OrdenesCombustibleDTO fuelOrderRequest in request.OrdenesCombustible)
            {
                OrdenesCombustible newFuelOrder = fuelOrdersPending.FirstOrDefault(f => f.OrdenCombustibleId == fuelOrderRequest.OrdenCombustibleId);

                if (newFuelOrder != null)
                {
                    if (!_ordenCombustibleDomainServices.TryToApplyFuelOrderToBoleta(newFuelOrder, boleta, out errorMessage))
                    {
                        return new OrdenesCombustibleDTO { MensajeError = errorMessage };
                    }
                }
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "AssignFuelOrderToBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCombustible();

            return new OrdenesCombustibleDTO();
        }

        public EliminarResponseDTO Delete(DeleteOrdenesCombustible request)
        {
            var ordenCombustible = _unidadDeTrabajo.OrdenesCombustible.Find(request.OrdenCombustibleId);

            if (ordenCombustible == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "OrdenCombustibleId NO Existe"
                };
            }

            if (!_ordenCombustibleDomainServices.TryRemoveFuelOrder(ordenCombustible, out string mensajeValidacion))
            {
                return new EliminarResponseDTO { MensajeError = mensajeValidacion };
            }

            _unidadDeTrabajo.OrdenesCombustible.Remove(ordenCombustible);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "EliminarOrdenCombustible");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCombustible();

            return new EliminarResponseDTO();
        }

        private bool ExistenDocumentosOrdenCombustible(string codigoFactura, int gasolineraId)
        {
            return !string.IsNullOrWhiteSpace(codigoFactura) ? 
                _unidadDeTrabajo.OrdenesCombustible.Any(c => c.CodigoFactura == codigoFactura && c.GasolineraCredito.Gasolinera.GasolineraId != gasolineraId) 
                : false;                       
        }

        private string VerificarCodigoFacturas(string codigoFactura, int gasolineraId, bool esActualizacion)
        {
            var error = string.Empty;
            var existeCodigoFactura = false;

            if (esActualizacion)
            {
                existeCodigoFactura = !string.IsNullOrWhiteSpace(codigoFactura) ? _unidadDeTrabajo
                                              .OrdenesCombustible.Any(r => r.CodigoFactura == codigoFactura 
                                              && r.GasCreditoId != gasolineraId) : false;
            }
            else
            {
                existeCodigoFactura = _unidadDeTrabajo.OrdenesCombustible.Any(r => r.CodigoFactura == codigoFactura);
            }

            if (existeCodigoFactura)
            {
                return string.Format("El Código de Factura {0} ya existe", codigoFactura);
            }

            return error;
        }

        private bool RemoveOldFuelOrders(Boletas boleta, List<OrdenesCombustibleDTO> ordenesCombustibleRequest, out string errorMessage)
        {
            List<OrdenesCombustible> currentFuelOrders = boleta.OrdenesCombustible.ToList();

            foreach (OrdenesCombustible fuelOrder in currentFuelOrders)
            {
                OrdenesCombustibleDTO orderDto = ordenesCombustibleRequest.FirstOrDefault(f => f.OrdenCombustibleId == fuelOrder.OrdenCombustibleId);

                if (orderDto != null) continue;

                if (!_ordenCombustibleDomainServices.TryRemoveFuelOrderFromBoleta(fuelOrder, out errorMessage))
                {
                    return false;
                }    
            }

            errorMessage = string.Empty;
            return true;
        }

        private void RemoverCacheOrdenesCombustible()
        {
            var listaKey = new List<string>
            {
                KeyCache.OrdenesCombustible,
                KeyCache.GasCreditos,
                KeyCache.BoletasDetalles,
                KeyCache.Boletas
            };

            _cacheAdapter.Remove(listaKey);
        }
    }
}
