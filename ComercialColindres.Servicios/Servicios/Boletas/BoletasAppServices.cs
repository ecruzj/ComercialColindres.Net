using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using System;
using ServidorCore.Funciones;

namespace ComercialColindres.Servicios.Servicios
{
    public class BoletasAppServices : IBoletasAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        private readonly IStorageService _storageService;
        ComercialColindresContext _unidadDeTrabajo;
        IBoletasDetalleDomainServices _boletasDetalleDomainSerives;
        IDescargadoresDomainServices _descargadoresDomainServices;
        IBoletasDomainServices _boletasDomainServices;
        IBoletaHumedadDomainServices _boletaHumedadDomainServices;
        IFacturaDetalleBoletaDomainServices _facturaDetalleBoletaDomainServices;

        public BoletasAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter, IStorageService storageService,
                                  IBoletasDetalleDomainServices boletasDetalleDomainSerives, 
                                  IDescargadoresDomainServices descargadoresDomainServices, 
                                  IBoletasDomainServices boletasDomainServices,
                                  IBoletaHumedadDomainServices boletaHumedadDomainServices, 
                                  IFacturaDetalleBoletaDomainServices facturaDetalleBoletaDomainServices)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
            _storageService = storageService;
            _boletasDetalleDomainSerives = boletasDetalleDomainSerives;
            _descargadoresDomainServices = descargadoresDomainServices;
            _boletasDomainServices = boletasDomainServices;
            _boletaHumedadDomainServices = boletaHumedadDomainServices;
            _facturaDetalleBoletaDomainServices = facturaDetalleBoletaDomainServices;
        }

        public EliminarResponseDTO Delete(DeleteBoleta request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "BoletaId NO Existe!"
                };
            }

            if (boleta.Estado == Estados.ACTIVO)
            {
                if (!_boletasDomainServices.TryEliminarBoleta(boleta, out string mensajeValidacion))
                {
                    return new EliminarResponseDTO { MensajeError = mensajeValidacion };
                }
            }
            
            var eliminarBoleta = boleta.GetValidationErrorsDelete();

            if (eliminarBoleta.Any())
            {
                return new EliminarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(eliminarBoleta)
                };
            }

            _unidadDeTrabajo.Boletas.Remove(boleta);

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "EliminarBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new EliminarResponseDTO();
        }

        public List<BoletasDTO> Get(GetBoletasPorValorBusqueda request)
        {
            var cacheKey = string.Format("{0}{1}{2}{3}", KeyCache.Boletas, "GetBoletasPorValorBusqueda", request.ValorBusqueda, request.PlantaId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = BoletasEspecificaciones.FiltrarPorPlantaBoletaFacturar(request.ValorBusqueda, request.PlantaId);
                var datos = _unidadDeTrabajo.Boletas.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Boletas>, IEnumerable<BoletasDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public List<BoletasDTO> Get(GetBoletasPendientesDeFacturar request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Boletas, "GetBoletasPendientesDeFacturar", request.PlantaId);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = BoletasEspecificaciones.FiltrarBoletasPendientesDeFacturar(request.PlantaId);
                var datos = _unidadDeTrabajo.Boletas.Where(especificacion.EvalFunc).ToList();
                var dtos = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Boletas>, IEnumerable<BoletasDTO>>(datos);
                return dtos.ToList();
            });

            return retorno;
        }

        public BusquedaBoletasDTO Get(GetByValorBoletas request)
        {
            try
            {
                var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
                var cacheKey = string.Format("{0}-{1}-{2}-{3}", KeyCache.Boletas, request.Filtro, request.PaginaActual, request.CantidadRegistros);
                var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
                {



                    var especificacion = BoletasEspecificaciones.FiltrarBoletasBusqueda(request.Filtro);
                    List<Boletas> datos = _unidadDeTrabajo.Boletas.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                    var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                    var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Boletas>, IEnumerable<BoletasDTO>>(datosPaginados.Items as IEnumerable<Boletas>);

                    var dto = new BusquedaBoletasDTO
                    {
                        PaginaActual = pagina,
                        TotalPagina = datosPaginados.TotalPagina,
                        TotalRegistros = datosPaginados.TotalRegistros,
                        Items = new List<BoletasDTO>(datosDTO)
                    };

                    return dto;

                });

                return datosConsulta;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<BoletasDTO> Get(GetBoletasInProcess request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.Boletas, "GetBoletasInProcess", request.RequestUserInfo.SucursalId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<Boletas> datos = _unidadDeTrabajo.Boletas.Where(r => r.Estado != Estados.CERRADO).OrderBy(o => o.FechaCreacionBoleta).ToList();
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<Boletas>, IEnumerable<BoletasDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public BoletasDTO Get(GetBoleta request)
        {
            var cacheKey = string.Format("{0}{1}", KeyCache.Boletas, request.BoletaNo);
            var retorno = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var dato = _unidadDeTrabajo.Boletas.Where(b => (b.CodigoBoleta == request.BoletaNo || 
                                                          b.NumeroEnvio == request.BoletaNo) &&
                                                          b.PlantaId == request.PlantaId).FirstOrDefault();
                if (dato == null)
                {
                    return new BoletasDTO
                    {
                        MensajeError = "BoletaNo NO existe"
                    };
                }
                var dto = AutomapperTypeAdapter.ProyectarComo<Boletas, BoletasDTO>(dato);
                return dto;
            });
            return retorno;
        }

        public bool ExisteBoleta(string codigoBoleta, string numeroEnvio, int plantaId)
        {
            ClientePlantas planta = _unidadDeTrabajo.ClientePlantas.FirstOrDefault(c => c.PlantaId == plantaId);

            if (planta == null) return false;

            bool evaluteShippingNumer = planta.IsShippingNumberRequired();

            return evaluteShippingNumer ?
                  _unidadDeTrabajo.Boletas.Any(b => b.NumeroEnvio == numeroEnvio.Trim() && b.PlantaId == plantaId)
                : _unidadDeTrabajo.Boletas.Any(b => b.CodigoBoleta == codigoBoleta.Trim() && b.PlantaId == plantaId);
        }

        public ActualizarResponseDTO Post(PostBoleta request)
        {
            var boletaRequest = request.Boleta;
            
            var boleta = _unidadDeTrabajo.Boletas.Find(boletaRequest.BoletaId);

            if (boleta != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "BoletaId ya Existe"
                };
            }            
            
            var productoPrecios = _unidadDeTrabajo.PrecioProductos.FirstOrDefault(p => p.CategoriaProductoId == request.Boleta.CategoriaProductoId);

            if (productoPrecios == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = string.Format("No se encuentran los Precios del Producto {0}", request.Boleta.DescripcionTipoProducto)
                };
            }

            ///save imagen in server
            //string pictureName = request.Boleta.CodigoBoleta;
            //const string boletaStorageRoot = "/apicomercialcolindres/db_images/boletas/";
            //var uploadResoult = _storageService.SaveFile(boletaStorageRoot, request.Boleta.Imagen, pictureName);

            //if (!uploadResoult.WasSuccessful)
            //{
            //    return new ActualizarResponseDTO
            //    {
            //        MensajeError = "No se pudo guardar la imagen en el server"
            //    };
            //}

            Proveedores vendor = _unidadDeTrabajo.Proveedores.FirstOrDefault(p => p.ProveedorId == request.Boleta.ProveedorId);
            boleta = new Boletas(request.Boleta.CodigoBoleta, request.Boleta.NumeroEnvio, vendor, request.Boleta.PlacaEquipo, request.Boleta.Motorista,
                                 request.Boleta.CategoriaProductoId, request.Boleta.PlantaId, request.Boleta.PesoEntrada, request.Boleta.PesoSalida, request.Boleta.CantidadPenalizada, 
                                 request.Boleta.Bonus, request.Boleta.PrecioProductoCompra, productoPrecios.PrecioVenta, request.Boleta.FechaSalida, request.Boleta.Imagen, "testPath");
            
            var mensajesValidacion = boleta.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            var validacionesBoleta = VerificarBoletas(boleta);

            if (validacionesBoleta.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionesBoleta)
                };
            }

            AsignarDescargaProducto(boleta);

            //Verifying OutStanding Boleta Humedad
            BoletaHumedad outStandingBoletaHumedad = _unidadDeTrabajo.BoletasHumedad.FirstOrDefault(h => h.PlantaId == boleta.PlantaId &&
                                                                                                    h.NumeroEnvio == boleta.NumeroEnvio);

            TryToAssignOutStandingHumidity(outStandingBoletaHumedad, boleta, out string errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            if (!TryToAssignToInvoice(boleta, out errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            //Bonus Product
            List<BonificacionProducto> bonusProduct = _unidadDeTrabajo.BonifacionProducto.ToList();

            if (!_boletasDomainServices.CanAssignBonusProduct(boleta, bonusProduct, out errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            _unidadDeTrabajo.Boletas.Add(boleta);

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CreacionBoletas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new ActualizarResponseDTO();
        }

        private bool TryToAssignToInvoice(Boletas boleta, out string errorValidation)
        {
            FacturaDetalleBoletas invoiceBoletaDetail = boleta.IsShippingNumberRequired()
                                                        ? _unidadDeTrabajo.FacturaDetalleBoletas.FirstOrDefault(b => b.Factura.PlantaId == boleta.PlantaId && b.NumeroEnvio == boleta.NumeroEnvio)
                                                        : _unidadDeTrabajo.FacturaDetalleBoletas.FirstOrDefault(b => b.Factura.PlantaId == boleta.PlantaId && b.CodigoBoleta == boleta.CodigoBoleta);

            return _facturaDetalleBoletaDomainServices.TryToAssignBoletaToInvoice(boleta, invoiceBoletaDetail, out errorValidation);
        }

        private void TryToAssignOutStandingHumidity(BoletaHumedad boletaHumidity, Boletas boleta, out string errorMessage)
        {
            _boletaHumedadDomainServices.TryToAssignOutStandingBoletaHumedadForPayment(boletaHumidity, boleta, out errorMessage);
        }

        private void AsignarDescargaProducto(Boletas boleta)
        {
            var descargaPorAdelantado = _unidadDeTrabajo.DescargasPorAdelantado.Where(d => d.PlantaId == boleta.PlantaId && d.Estado == Estados.PENDIENTE).ToList();

            if (descargaPorAdelantado != null)
            {
                _descargadoresDomainServices.TryAsignarDescargaProductoPorAdelantado(boleta, descargaPorAdelantado);
            }
        }

        private List<string> VerificarBoletas(Boletas boleta)
        {
            var listaErrores = new List<string>();
            var existeCodigoBoleta = boleta.BoletaId > 0
                                     ? _unidadDeTrabajo.Boletas.Any(r => r.CodigoBoleta == boleta.CodigoBoleta && r.PlantaId == boleta.PlantaId && r.BoletaId != boleta.BoletaId)
                                     : _unidadDeTrabajo.Boletas.Any(r => r.CodigoBoleta == boleta.CodigoBoleta && r.PlantaId == boleta.PlantaId);
            var existeNuevoEnvio = boleta.BoletaId > 0
                                   ? !string.IsNullOrWhiteSpace(boleta.NumeroEnvio) ? _unidadDeTrabajo.Boletas.Any(c => c.NumeroEnvio == boleta.NumeroEnvio && c.PlantaId == boleta.PlantaId && c.BoletaId != boleta.BoletaId) : false
                                   : !string.IsNullOrWhiteSpace(boleta.NumeroEnvio) ? _unidadDeTrabajo.Boletas.Where(c => c.NumeroEnvio == boleta.NumeroEnvio && c.PlantaId == boleta.PlantaId).Any() : false;
            
            if (existeCodigoBoleta)
            {
                var mensaje = string.Format("El #Boleta {0} ya existe", boleta.CodigoBoleta);
                listaErrores.Add(mensaje);
            }

            if (existeNuevoEnvio)
            {
                var mensaje = string.Format("El #Envío {0} ya existe", boleta.NumeroEnvio);
                listaErrores.Add(mensaje);
            }

            return listaErrores;
        }

        public ActualizarResponseDTO Put(PutBoleta request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.Boleta.BoletaId);
            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "BoletaIdd No Existe"
                };
            }

            if (boleta.Estado == Estados.CERRADO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La Boleta ya esta Cerrada"
                };
            }

            boleta.ActualizarBoleta(request.Boleta.PlantaId, request.Boleta.CodigoBoleta, request.Boleta.NumeroEnvio, request.Boleta.ProveedorId, 
                                    request.Boleta.PlacaEquipo, request.Boleta.Motorista, request.Boleta.CategoriaProductoId, request.Boleta.PesoEntrada,
                                    request.Boleta.PesoSalida, request.Boleta.CantidadPenalizada, request.Boleta.Bonus, request.Boleta.PrecioProductoCompra, 
                                    request.Boleta.FechaSalida, request.Boleta.Imagen);

            var mensajesValidacion = boleta.GetValidationErrors();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            var validacionesBoleta = VerificarBoletas(boleta);

            if (validacionesBoleta.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionesBoleta)
                };
            }

            //Verifying OutStanding Boleta Humedad
            BoletaHumedad outStandingBoletaHumedad = _unidadDeTrabajo.BoletasHumedad.FirstOrDefault(h => h.PlantaId == boleta.PlantaId &&
                                                                                                    h.NumeroEnvio == boleta.NumeroEnvio);

            TryToAssignOutStandingHumidity(outStandingBoletaHumedad, boleta, out string errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            if (!TryToAssignToInvoice(boleta, out errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            //Bonus Product
            List<BonificacionProducto> bonusProduct = _unidadDeTrabajo.BonifacionProducto.ToList();

            if (!_boletasDomainServices.CanAssignBonusProduct(boleta, bonusProduct, out errorMessage))
            {
                return new ActualizarResponseDTO { MensajeError = errorMessage };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizacionBoletas");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new ActualizarResponseDTO();
        }

        public BoletasDTO UpdateBoletaProperties(UpdateBoletaProperties request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.Boleta.BoletaId);

            if (boleta == null) { return new BoletasDTO { MensajeError = "La Boleta no existe!" }; }

            BoletasDTO boletaRequest = request.Boleta;


            Proveedores vendor = _unidadDeTrabajo.Proveedores.FirstOrDefault(p => p.ProveedorId == request.Boleta.ProveedorId);
            CategoriaProductos product = _unidadDeTrabajo.CategoriaProductos.FirstOrDefault(p => p.CategoriaProductoId == boletaRequest.CategoriaProductoId);
            ClientePlantas factory = _unidadDeTrabajo.ClientePlantas.FirstOrDefault(p => p.PlantaId == boletaRequest.PlantaId);

            if (!_boletasDomainServices.TryUpdateProperties(boleta, boletaRequest.CodigoBoleta, boletaRequest.NumeroEnvio, vendor, boletaRequest.PlacaEquipo, 
                                                            boletaRequest.Motorista, product, factory, boletaRequest.PesoEntrada, boletaRequest.PesoSalida, 
                                                            boletaRequest.CantidadPenalizada, boletaRequest.Bonus, boletaRequest.PrecioProductoCompra, 
                                                            boletaRequest.FechaSalida, boletaRequest.Imagen, out string mensajeValidacion))
            {
                return new BoletasDTO { MensajeError = mensajeValidacion };
            }

            IEnumerable<string> errors = boleta.GetValidationErrors();

            if (errors.Any())
            {
                return new BoletasDTO { MensajeError = errors.FirstOrDefault() };
            }

            if (!TryToAssignToInvoice(boleta, out mensajeValidacion))
            {
                return new BoletasDTO { MensajeError = mensajeValidacion };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "UpdateBoletaProperties");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new BoletasDTO();
        }

        public ActualizarResponseDTO Put(PutCierreForzadoBoleta request)
        {
            var boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (boleta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La BoletaId NO Existe"
                };
            }

            List<Deducciones> deductions = _unidadDeTrabajo.Deducciones.ToList();

            if (!_boletasDomainServices.TryCierreForzado(boleta, deductions, out string mensajeValidacion))
            {
                return new ActualizarResponseDTO { MensajeError = mensajeValidacion };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CierreForzadoBoleta");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new ActualizarResponseDTO();
        }

        public BoletasDTO OpenBoletaById(OpenBoletaById request)
        {
            if (request.RequestUserInfo == null) throw new ArgumentNullException(nameof(request.RequestUserInfo));

            Boletas boleta = _unidadDeTrabajo.Boletas.Find(request.BoletaId);

            if (!_boletasDomainServices.TryOpenBoletaById(boleta, out string errorMessage))
            {
                return new BoletasDTO { MensajeError = errorMessage };
            }

            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.RequestUserInfo.UserId, "OpenBoletaById");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheBoletas();

            return new BoletasDTO();
        }

        private void RemoverCacheBoletas()
        {
            var listaKey = new List<string>
            {
                KeyCache.Boletas,
                KeyCache.GasCreditos,
                KeyCache.Prestamos,
                KeyCache.PagoPrestamos,
                KeyCache.BoletasHumedadPago,
                KeyCache.BoletasHumedadAsignacion,
                KeyCache.BoletasHumedad,
                KeyCache.Facturas,
                KeyCache.FacturaDetalleBoletas,
                KeyCache.AjusteBoletas,
                KeyCache.AjusteBoletaDetalles,
                KeyCache.AjusteBoletaPagos,
                KeyCache.Descargadores,
                KeyCache.DescargasPorAdelantado,
                KeyCache.LineasCredito,
                KeyCache.LineasCreditoDeducciones
            };
            _cacheAdapter.Remove(listaKey);
        }
    }
}
