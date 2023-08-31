using ComercialColindres.DTOs.Clases;

namespace ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems
{
    public class FacturaDetalleItemDto : BaseDTO
    {
        public int FacturaDetalleItemId { get; set; }
        public int FacturaId { get; set; }
        public decimal Cantidad { get; set; }
        public int CategoriaProductoId { get; set; }
        public decimal Precio { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalInvoice { get; set; }
        public string ProductoDescripcion { get; set; }
        public decimal LocalCurrencyItemAmount { get; set; }
        public decimal ForeignCurrencyItemAmount { get; set; }
        public bool IsForeignCurrency { get; set; }
        public bool IsExonerated { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxItem { get; set; }
    }
}
