using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices.Invoice
{
    public class InvoiceDomainServices : IInvoiceDomainServices
    {
        public InvoiceDomainServices()
        {

        }

        public bool CanUpdateInvoice(Factura invoice, List<Factura> invoices, string invoiceNoRequest, ClientePlantas factory, SubPlanta subFactory, out string errorMessage)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (invoices == null) throw new ArgumentNullException(nameof(invoices));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            if (invoice.Estado != Estados.NUEVO)
            {
                errorMessage = "Unicamente puede editar Facturas en estado Nuevo";
                return false;
            }

            bool existeFactura = invoices.Any(f => f.NumeroFactura == invoiceNoRequest && f.FacturaId != invoice.FacturaId);

            if (existeFactura)
            {
                errorMessage = "El Número de Factura ya existe";
                return false;
            }

            if (factory.HasSubPlanta && subFactory == null)
            {
                errorMessage = "Debes seleccionar una SubPlanta";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool IsAblePurchaseOrder(string po, Factura invoice, List<Factura> invoices, ClientePlantas factory, out string errorMessage)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (invoices == null) throw new ArgumentNullException(nameof(invoices));

            if (factory.RequiresPurchaseOrder)
            {
                bool isUnAblePo = invoices.Any(f => f.OrdenCompra == po && f.FacturaId != invoice.FacturaId && f.Estado != Estados.ANULADO);

                if (isUnAblePo)
                {
                    errorMessage = "La orden de compra ya existe en otra Factura";
                    return false;
                }
            }
            
            errorMessage = string.Empty;
            return true;
        }

        public bool IsAbleExemptionNo(string exemptionNo, Factura invoice, List<Factura> invoices, ClientePlantas factory, out string errorMessage)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (invoices == null) throw new ArgumentNullException(nameof(invoices));

            if (factory.HasExonerationNo)
            {
                bool isUnAbleExemotionNo = invoices.Any(f => f.ExonerationNo == exemptionNo && f.FacturaId != invoice.FacturaId && f.Estado != Estados.ANULADO);

                if (isUnAbleExemotionNo)
                {
                    errorMessage = "La ExoneraciónNo de compra ya existe en otra Factura";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
