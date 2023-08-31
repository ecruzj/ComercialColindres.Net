using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IFacturaDetalleBoletaDomainServices
    {
        bool TryToAssignBoletaToInvoice(Boletas boleta, FacturaDetalleBoletas invoiceBoletaDetail, out string errorValidation);
        bool CanAssignBoletasDetail(Factura invoice, out string errorValidation);
    }
}
