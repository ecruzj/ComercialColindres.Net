using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems
{
    [Route("/factura-detalle-items/{invoiceId}", "GET")]
    public class GetDetailItemsByInvoiceId : RequestBase, IReturn<List<FacturaDetalleItemDto>>
    {
        public int InvoiceId { get; set; }
    }

    [Route("/factura-detalle-items/", "POST")]
    public class PostInvoiceDetailItems : RequestBase, IReturn<FacturaDetalleItemDto>
    {
        public int InvoiceId { get; set; }
        public List<FacturaDetalleItemDto> FacturaDetalleItems { get; set; }
    }
}
