using ComercialColindres.DTOs.RequestDTOs.NotasCredito;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface INotaCreditoServices
    {
        List<NotaCreditoDto> GetNotasCreditoByInvoiceId(GetNotasCreditoByInvoiceId request);
        NotaCreditoDto SaveNotasCredito(PostNotasCredito request);
    }
}
