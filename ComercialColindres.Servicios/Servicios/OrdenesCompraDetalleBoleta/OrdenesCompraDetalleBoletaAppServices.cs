using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Servicios.Recursos;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.Servicios.Servicios
{
    public class OrdenesCompraDetalleBoletaAppServices : IOrdenesCompraDetalleBoletaAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public OrdenesCompraDetalleBoletaAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public List<OrdenesCompraDetalleBoletaDTO> Get(GetDetalleBoletasPorOrdenCompraId request)
        {
            var cacheKey = string.Format("{0}{1}{2}", KeyCache.OrdenescompraDetalleBoleta, "GetDetalleBoletasPorOrdenCompraId", request.OrdenCompraId);
            var datosConsulta = _cacheAdapter.TryGetValue(cacheKey, () =>
            {
                List<OrdenesCompraDetalleBoleta> datos = _unidadDeTrabajo.OrdenesCompraDetalleBoleta.Where(d => d.OrdenCompraProductoId == request.OrdenCompraId)
                                                                            .OrderByDescending(o => o.Boleta.CategoriaProductoId).ToList();

                var datosDTO =
                    AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<OrdenesCompraDetalleBoleta>, IEnumerable<OrdenesCompraDetalleBoletaDTO>>(datos);
                return datosDTO.ToList();
            });

            return datosConsulta;
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
