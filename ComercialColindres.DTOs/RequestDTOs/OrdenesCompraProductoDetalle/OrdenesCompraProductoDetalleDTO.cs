using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle
{
    public class OrdenesCompraProductoDetalleDTO : BaseDTO
    {
        public int OrdenCompraProductoDetalleId { get; set; }
        public int OrdenCompraProductoId { get; set; }
        public int BiomasaId { get; set; }
        public decimal Toneladas { get; set; }
        public decimal PrecioDollar { get; set; }
        public decimal ConversionDollarToLps { get; set; }

        public string OrdenCompraProductoNo { get; set; }
        public string MaestroBiomasaDescripcion { get; set; }
        public decimal TotalDollares { get; set; }
        public decimal TotalLps { get; set; }
        public decimal PrecioLPS { get; set; }
        public decimal ToneladasAsignadas { get; set; }
        public decimal CumplimientoDetalleEntrega { get; set; }
    }
}
