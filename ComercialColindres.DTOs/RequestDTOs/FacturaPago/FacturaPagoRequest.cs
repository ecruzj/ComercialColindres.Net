using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs
{
    [Route("/factura-pago/{invoiceId}", "GET")]
    public class GetPagosByInvoiceId : RequestBase, IReturn<List<FacturaPagoDto>>
    {
        public int InvoiceId { get; set; }
    }

    [Route("/factura-pago/", "POST")]
    public class PostInvoicePagos : RequestBase, IReturn<FacturaPagoDto>
    {
        public int InvoiceId { get; set; }
        public List<FacturaPagoDto> FacturaPagos { get; set; }
    }
}
