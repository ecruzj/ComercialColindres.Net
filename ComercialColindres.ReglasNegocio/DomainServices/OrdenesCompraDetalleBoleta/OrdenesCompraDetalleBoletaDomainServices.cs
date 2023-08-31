using System;
using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class OrdenesCompraDetalleBoletaDomainServices : IOrdenesCompraDetalleBoletaDomainServices
    {
        public void AgregarBoletaOrdenCompraProducto(OrdenesCompraProducto ordenCompraProducto, Boletas boleta)
        {
            var ordenDetalleBoleta = new OrdenesCompraDetalleBoleta(ordenCompraProducto.OrdenCompraProductoId, boleta.BoletaId, boleta.PesoProducto);

            ordenCompraProducto.OrdenesCompraDetalleBoletas.Add(ordenDetalleBoleta);
        }

        public bool TryValidateInvoiceToOrdenProducto(Factura factura, OrdenesCompraProducto ordenCompraProducto)
        {
            if (factura == null) throw new ArgumentNullException("factura");
            if (ordenCompraProducto == null) throw new ArgumentNullException("ordenCompraProducto");

            //return factura.OrdenCompraProductoId == ordenCompraProducto.OrdenCompraProductoId;
            return false;
        }
    }
}
