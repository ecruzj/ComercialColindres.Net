using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Especificaciones;
using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using ServidorCore.Aplicacion;
using ComercialColindres.Datos.Recursos;

namespace ComercialColindres.Servicios.Servicios
{
    public class OrdenesCompraProductoAppServices : IOrdenesCompraProductoAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public OrdenesCompraProductoAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public BusquedaOrdenesCompraProductoDTO Get(GetByValorOrdenesComprasProducto request)
        {
            var pagina = request.PaginaActual == 0 ? 1 : request.PaginaActual;
            var cacheKey = string.Format("{0}{1}{2}{3}{4}", KeyCache.OrdenescompraProducto, "GetByValorOrdenesComprasProducto", request.Filtro, 
                                                               request.PaginaActual, request.CantidadRegistros);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var especificacion = OrdenesCompraProductoEspecificaciones.FiltrarOrdenesCompraProductos(request.Filtro);
                List<OrdenesCompraProducto> datos = _unidadDeTrabajo.OrdenesCompraProducto.Where(especificacion.EvalFunc).OrderByDescending(o => o.FechaTransaccion).ToList();
                var datosPaginados = datos.Paginar(pagina, request.CantidadRegistros);

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCompraProducto>, IEnumerable<OrdenesCompraProductoDTO>>(datosPaginados.Items as IEnumerable<OrdenesCompraProducto>);

                var dto = new BusquedaOrdenesCompraProductoDTO
                {
                    PaginaActual = pagina,
                    TotalPagina = datosPaginados.TotalPagina,
                    TotalRegistros = datosPaginados.TotalRegistros,
                    Items = new List<OrdenesCompraProductoDTO>(datosDTO)
                };

                return dto;
            });

            return datosConsulta;
        }

        public OrdenesCompraProductoDTO Get(GetDatosPOPorPlantaId request)
        {
            var cacheKey = string.Format("{0}-{1}-{2}", KeyCache.OrdenescompraProducto, "GetByValorOrdenesComprasProducto", request.PlantaId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                var datos = _unidadDeTrabajo.OrdenesCompraProducto.SingleOrDefault(po => po.PlantaId == request.PlantaId
                                                                                   && po.EsOrdenCompraActual
                                                                                   && po.Estado == Estados.ACTIVO);
                var datosDTO =
                    AutomapperTypeAdapter.ProyectarComo<OrdenesCompraProducto, OrdenesCompraProductoDTO>(datos);

                return datosDTO;
            });
            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostOrdenCompraProducto request)
        {
            var ordenCompraProductoRequest = request.OrdenCompraProducto;
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(ordenCompraProductoRequest.OrdenCompraProductoId);

            if (ordenCompraProducto != null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "YA Existe la OrdenCompraProductoId"
                };
            }

            var plantaRequiereOrdenCompra = PlantaRequiereOrdenCompraProducto(ordenCompraProductoRequest.PlantaId);

            if (!plantaRequiereOrdenCompra)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "PlantaId no está configurada con Requerimiento de Orden de Compra de Productos"
                };
            }

            var existeOrdenCompraNo = ExisteOrdenCompraProductoNo(ordenCompraProductoRequest.OrdenCompraProductoId, ordenCompraProductoRequest.OrdenCompraNo,
                                                                  ordenCompraProductoRequest.PlantaId, false);

            if (existeOrdenCompraNo)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Ya existe el # de la Orden Compra Producto"
                };
            }

            var existeExoneracionDei = ExisteExoneracionDEI(ordenCompraProductoRequest.OrdenCompraProductoId, ordenCompraProductoRequest.NoExoneracionDEI,
                                                            ordenCompraProductoRequest.PlantaId, false);

            if (existeExoneracionDei)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Ya existe el # de Exoneración de la DEI"
                };
            }

            ordenCompraProducto = new OrdenesCompraProducto(ordenCompraProductoRequest.OrdenCompraNo, ordenCompraProductoRequest.NoExoneracionDEI,
                                                            ordenCompraProductoRequest.PlantaId, ordenCompraProductoRequest.FechaCreacion,
                                                            ordenCompraProductoRequest.CreadoPor, ordenCompraProductoRequest.MontoDollar,
                                                            ordenCompraProductoRequest.ConversionDollarToLps);

            var mensajesValidacion = ordenCompraProducto.GetValidationErrors().ToList();
            if (mensajesValidacion.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                };
            }

            _unidadDeTrabajo.OrdenesCompraProducto.Add(ordenCompraProducto);            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UsuarioId, "CreacionOrdenesCompraProducto");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new ActualizarResponseDTO();
        }
        
        public ActualizarResponseDTO Put(PutCerrarOrdenCompraProducto request)
        {
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(request.OrdenCompraProductoId);

            if (ordenCompraProducto == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "OrdenCompraProductoId NO Existe!"
                };
            }

            var cerrarOrdenCompraProducto = ordenCompraProducto.CerrarOrdenCompraProducto();

            if (!string.IsNullOrWhiteSpace(cerrarOrdenCompraProducto))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = cerrarOrdenCompraProducto
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "CerrarOrdenCompraProducto");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutActivarOrdenCompraProducto request)
        {
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(request.OrdenCompraProductoId);

            if (ordenCompraProducto == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "OrdenCompraProductoId NO Existe!"
                };
            }

            var planta = _unidadDeTrabajo.ClientePlantas.Find(request.PlantaId);

            if (planta == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "La PlantaId NO Exite!"
                };
            }

            var existeOrdenCompraActiva = ExisteOrdenCompraActiva(planta.PlantaId);

            if (existeOrdenCompraActiva)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Actulamente existe una Orden de Compra de Producto Activa"
                };
            }

            var activarOrdenCompra = ordenCompraProducto.ActivarOrdenCompraProducto();

            if (!string.IsNullOrWhiteSpace(activarOrdenCompra))
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = activarOrdenCompra
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActivarOrdenCompraProducto");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new ActualizarResponseDTO();
        }

        public ActualizarResponseDTO Put(PutOrdenCompraProducto request)
        {
            var ordenCompraProductoRequest = request.OrdenCompraProducto;
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(ordenCompraProductoRequest.OrdenCompraProductoId);

            if (ordenCompraProducto == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "OrdenCompraProductoId NO Existe!"
                };
            }

            var existeOrdenCompraNo = ExisteOrdenCompraProductoNo(ordenCompraProductoRequest.OrdenCompraProductoId, ordenCompraProductoRequest.OrdenCompraNo,
                                                                  ordenCompraProductoRequest.PlantaId, true);

            if (existeOrdenCompraNo)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Ya existe el # de la Orden Compra Producto"
                };
            }

            var existeExoneracionDei = ExisteExoneracionDEI(ordenCompraProductoRequest.OrdenCompraProductoId, ordenCompraProductoRequest.NoExoneracionDEI,
                                                            ordenCompraProductoRequest.PlantaId, true);

            if (existeExoneracionDei)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Ya existe el # de Exoneración de la DEI"
                };
            }

            ordenCompraProducto.ActualizarOrdenCompraProducto(ordenCompraProductoRequest.OrdenCompraNo,
                                                              ordenCompraProductoRequest.NoExoneracionDEI, ordenCompraProductoRequest.FechaCreacion,
                                                              ordenCompraProductoRequest.MontoDollar, ordenCompraProductoRequest.ConversionDollarToLps);

            var validacionesOrdenCompra = ordenCompraProducto.GetValidationErrors();

            if (validacionesOrdenCompra.Any())
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionesOrdenCompra)
                };
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ActualizarOrdenCompraProducto");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new ActualizarResponseDTO();
        }

        public EliminarResponseDTO Delete(DeleteOrdenCompraProducto request)
        {
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(request.OrdenCompraProductoId);

            if (ordenCompraProducto == null)
            {
                return new EliminarResponseDTO
                {
                    MensajeError = "OrdenCompraProductoId NO Existe!"
                };
            }

            var validacionEliminarOrdenCompra = ordenCompraProducto.GetValidationErrorsDelete();

            if (validacionEliminarOrdenCompra.Any())
            {
                return new EliminarResponseDTO
                {
                    MensajeError = Utilitarios.CrearMensajeValidacion(validacionEliminarOrdenCompra)
                };
            }

            var detalleCompraProductos = ordenCompraProducto.OrdenesCompraProductoDetalles.ToList();

            foreach (var detalleProducto in detalleCompraProductos)
            {
                ordenCompraProducto.OrdenesCompraProductoDetalles.Remove(detalleProducto);
            }

            _unidadDeTrabajo.OrdenesCompraProducto.Remove(ordenCompraProducto);
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "EliminarOrdenCompraProducto");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new EliminarResponseDTO();
        }

        private bool PlantaRequiereOrdenCompraProducto(int plantaId)
        {
            var clientePlanta = _unidadDeTrabajo.ClientePlantas.Find(plantaId);

            if (clientePlanta == null)
            {
                return false;
            }

            return clientePlanta.RequiresPurchaseOrder;
        }

        private bool ExisteOrdenCompraProductoNo(int ordenCompraProductoId, string ordenCompraProductoNo, int plantaId, bool esActualizacion)
        {
            if (esActualizacion)
            {
                return _unidadDeTrabajo.OrdenesCompraProducto.Any(o => o.OrdenCompraNo == ordenCompraProductoNo && o.PlantaId == plantaId
                                                                    && o.OrdenCompraProductoId != ordenCompraProductoId);
            }

            return _unidadDeTrabajo.OrdenesCompraProducto.Any(o => o.OrdenCompraNo == ordenCompraProductoNo && o.PlantaId == plantaId);
        }

        private bool ExisteExoneracionDEI(int ordenCompraProductoId, string exoneracionDeiNo, int plantaId, bool esActualizacion)
        {
            if (esActualizacion)
            {
                return _unidadDeTrabajo.OrdenesCompraProducto.Any(o => o.NoExoneracionDEI == exoneracionDeiNo && o.PlantaId == plantaId
                                                                    && o.OrdenCompraProductoId != ordenCompraProductoId);
            }

            return _unidadDeTrabajo.OrdenesCompraProducto.Any(o => o.NoExoneracionDEI == exoneracionDeiNo && o.PlantaId == plantaId);
        }

        private bool ExisteOrdenCompraActiva(int plantaId)
        {
            var ordenCompraActiva = _unidadDeTrabajo.OrdenesCompraProducto.Any(op => op.EsOrdenCompraActual == true && op.PlantaId == plantaId);

            return ordenCompraActiva;
        }

        private void RemoverCacheOrdenesCompra()
        {
            var listaKey = new List<string>
            {
                KeyCache.OrdenescompraProducto,
                KeyCache.OrdenescompraProductoDetalle,
                KeyCache.OrdenescompraDetalleBoleta
            };
            _cacheAdapter.Remove(listaKey);
        }        
    }
}
