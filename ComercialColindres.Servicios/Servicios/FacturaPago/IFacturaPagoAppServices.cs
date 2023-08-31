using ComercialColindres.DTOs.RequestDTOs;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IFacturaPagoAppServices
    {
        List<FacturaPagoDto> GetInvoicePaymentByInvoiceId(GetPagosByInvoiceId request);
        FacturaPagoDto SaveInvoicePayment(PostInvoicePagos request);
    }
}
