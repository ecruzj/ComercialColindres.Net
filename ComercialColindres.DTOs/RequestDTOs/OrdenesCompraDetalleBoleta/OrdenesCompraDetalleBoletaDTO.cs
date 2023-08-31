using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta
{
    public class OrdenesCompraDetalleBoletaDTO : BaseDTO
    {
        public int OrdenCompraDetalleBoletaId { get; set; }
        public int OrdenCompraProductoId { get; set; }
        public int BoletaId { get; set; }
        public decimal CantidadFacturada { get; set; }

        public string OrdenCompraProductoNo { get; set; }
        public string CodigoBoleta { get; set; }
    }
}
