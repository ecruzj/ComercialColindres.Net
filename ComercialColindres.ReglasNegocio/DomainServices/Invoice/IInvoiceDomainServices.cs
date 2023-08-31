using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices.Invoice
{
    public interface IInvoiceDomainServices
    {
        bool CanUpdateInvoice(Factura invoice, List<Factura> invoinces, string invoiceNo, ClientePlantas factory, SubPlanta subFactory, out string errorMessage);
        bool IsAblePurchaseOrder(string po, Factura invoice, List<Factura> invoices, ClientePlantas factory, out string errorMessage);
        bool IsAbleExemptionNo(string exemptionNo, Factura invoice, List<Factura> invoices, ClientePlantas factory, out string errorMessage);
    }
}
