using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class FacturaDetalleBoletaDomainServices : IFacturaDetalleBoletaDomainServices
    {
        public bool TryToAssignBoletaToInvoice(Boletas boleta, FacturaDetalleBoletas invoiceBoletaDetail, out string errorValidation)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            errorValidation = string.Empty;

            if (invoiceBoletaDetail == null) return true;

            if (boleta.IsAssignedToInvoice()) return true;

            if (boleta.IsShippingNumberRequired())
            {
                if (boleta.NumeroEnvio == invoiceBoletaDetail.NumeroEnvio && invoiceBoletaDetail.BoletaId != null)
                {
                        errorValidation = "Existe una Boleta con el mismo #Envío asignada a una Factura";
                        return false;
                    
                }
            }
            else
            {
                if (boleta.CodigoBoleta == invoiceBoletaDetail.CodigoBoleta && invoiceBoletaDetail.BoletaId != null)
                {
                    errorValidation = "Existe una Boleta con el mismo Códigos asignada a una Factura";
                    return false;
                }
            }

            if (boleta.PlantaId == invoiceBoletaDetail.Factura.PlantaId)
            {
                invoiceBoletaDetail.AssignBoleta(boleta);
            }

            return true;
        }

        public bool CanAssignBoletasDetail(Factura invoice, out string errorValidation)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));

            List<FacturaDetalleBoletas> boletasDetail = invoice.FacturaDetalleBoletas.ToList();

            if (invoice.Estado != Estados.ACTIVO)
            {
                if (invoice.Estado == Estados.CERRADO && boletasDetail.Any())
                {
                    errorValidation = "La factura ya esta cerrada y cuenta con detalle de boletas!";
                    return false;
                }

                if (invoice.Estado == Estados.CERRADO && !boletasDetail.Any())
                {
                    errorValidation = string.Empty;
                    return true;
                }

                errorValidation = "El estado de la Factura debe ser ACTIVO!";
                return false;
            }

            errorValidation = string.Empty;
            return true;
        }
    }
}
