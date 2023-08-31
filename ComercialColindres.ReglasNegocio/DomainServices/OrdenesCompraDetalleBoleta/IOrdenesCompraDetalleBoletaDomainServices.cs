using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IOrdenesCompraDetalleBoletaDomainServices
    {
        bool TryValidateInvoiceToOrdenProducto(Factura factura, OrdenesCompraProducto ordenCompraProducto);
        void AgregarBoletaOrdenCompraProducto(OrdenesCompraProducto ordenCompraProducto, Boletas boleta);
    }
}
