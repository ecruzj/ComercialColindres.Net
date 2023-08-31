using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle;
using ComercialColindres.DTOs.ResponseDTOs;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ServidorCore.Aplicacion;

namespace ComercialColindres.Servicios.Servicios
{
    public class OrdenesCompraProductoDetalleAppServices : IOrdenesCompraProductoDetalleAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public OrdenesCompraProductoDetalleAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<OrdenesCompraProductoDetalleDTO> Get(GetOrdenesCompraProductoDetallePorPO request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.OrdenescompraProductoDetalle, "GetOrdenesCompraProductoDetallePorPO", request.OrdenCompraProductoId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<OrdenesCompraProductoDetalle> datos = _unidadDeTrabajo.OrdenesCompraProductoDetalle.Where(b => b.OrdenCompraProductoId == request.OrdenCompraProductoId)
                                                                            .OrderByDescending(o => o.MaestroBiomasa.Descripcion).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCompraProductoDetalle>, IEnumerable<OrdenesCompraProductoDetalleDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
        }

        public ActualizarResponseDTO Post(PostOrdenesCompraProductoDetalle request)
        {
            var ordenCompraProducto = _unidadDeTrabajo.OrdenesCompraProducto.Find(request.OrdenCompraProductoId);

            if (ordenCompraProducto == null)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "OrdenCompraProducto NO Existe!"
                };
            }

            if (ordenCompraProducto.Estado != Estados.NUEVO)
            {
                return new ActualizarResponseDTO
                {
                    MensajeError = "Solo puede configurar Ordenes de Compra de Producto en estado NUEVOS!"
                };
            }

            RemoverOrdenCompraProductoDetalle(ordenCompraProducto, request.OrdenesCompraProductoDetalle);
            var ordenCompraProductoDetalle = ordenCompraProducto.OrdenesCompraProductoDetalles.ToList();

            IEnumerable<string> mensajesValidacion;

            foreach (var ordenDetalle in request.OrdenesCompraProductoDetalle)
            {
                var productoBiomasa = ordenCompraProductoDetalle.FirstOrDefault(d => d.OrdenCompraProductoDetalleId == ordenDetalle.OrdenCompraProductoDetalleId);

                //Hubo una Actualización en el detalle del Producto Biomasa
                if (productoBiomasa != null)
                {
                    var existeCategoriaBiomasa = ExisteCategoriaBiomasa(ordenDetalle.BiomasaId);

                    if (!existeCategoriaBiomasa)
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = "No existe la Categoría BiomsaId"
                        };
                    }

                    productoBiomasa.ActualizarOrdenProductoDetalle(ordenDetalle.BiomasaId, ordenDetalle.Toneladas, ordenDetalle.PrecioDollar, ordenDetalle.PrecioLPS);

                    mensajesValidacion = productoBiomasa.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }
                }
                else
                {
                    //se creó un nuevo detalle de producto Biomasa
                    var existeCategoriaBiomasa = ExisteCategoriaBiomasa(ordenDetalle.BiomasaId);

                    if (!existeCategoriaBiomasa)
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = "No existe la Categoría BiomsaId"
                        };
                    }

                    var nuevoProductoDetalle = new OrdenesCompraProductoDetalle(ordenDetalle.OrdenCompraProductoId, ordenDetalle.BiomasaId,
                                                                                ordenDetalle.Toneladas, ordenDetalle.PrecioDollar, ordenDetalle.PrecioLPS);

                    mensajesValidacion = nuevoProductoDetalle.GetValidationErrors();

                    if (mensajesValidacion.Any())
                    {
                        return new ActualizarResponseDTO
                        {
                            MensajeError = Utilitarios.CrearMensajeValidacion(mensajesValidacion)
                        };
                    }

                    _unidadDeTrabajo.OrdenesCompraProductoDetalle.Add(nuevoProductoDetalle);
                }
            }
            
            var transaccion = TransactionInfoFactory.CrearTransactionInfo(request.UserId, "ConfiguracionOrdenesCompraDetalle");
            _unidadDeTrabajo.Commit(transaccion);

            RemoverCacheOrdenesCompra();

            return new ActualizarResponseDTO();
        }

        private void RemoverOrdenCompraProductoDetalle(OrdenesCompraProducto ordenCompraProducto,
                                                       List<OrdenesCompraProductoDetalleDTO> ordenCompraProductoDetallesRequest)
        {
            var ordenesCompraProductoDetalles = ordenCompraProducto.OrdenesCompraProductoDetalles.ToList();

            foreach (var detalle in ordenesCompraProductoDetalles)
            {
                var productoDetalle = ordenCompraProductoDetallesRequest.FirstOrDefault(d => d.OrdenCompraProductoDetalleId == detalle.OrdenCompraProductoDetalleId);

                if (productoDetalle == null)
                {
                    ordenCompraProducto.OrdenesCompraProductoDetalles.Remove(detalle);
                }
            }
        }
        
        private bool ExisteCategoriaBiomasa(int biomasaId)
        {
            var biomasa = _unidadDeTrabajo.MaestroBiomasa.Any(b => b.BiomasaId == biomasaId);
            return biomasa;
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
