using ComercialColindres.DTOs.Clases;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.NotasCredito
{
    [Route("/notas-credito/{invoiceId}", "GET")]
    public class GetNotasCreditoByInvoiceId : RequestBase, IReturn<List<NotaCreditoDto>>
    {
        public int InvoiceId { get; set; }
    }

    [Route("/notas-credito/", "POST")]
    public class PostNotasCredito : RequestBase, IReturn<NotaCreditoDto>
    {
        public int InvoiceId { get; set; }
        public List<NotaCreditoDto> NotasCredito { get; set; }
    }
}
