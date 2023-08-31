using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IAjusteBoletaDomainServices
    {
        bool TryCreateBoletaAjuste(Boletas boleta, out string errorMessage);
        bool TryCreateBoletaAjusteDetalle(AjusteBoleta ajusteBoleta, AjusteTipo ajusteTipo, decimal monto, string observaciones, LineasCredito lineaCredito, string noDocumento, out string errorMessage);
        bool TryDeleteAjusteBoleta(AjusteBoleta ajusteBoleta, out string errorMessage);
        bool TryDeleteAjusteDetalle(AjusteBoletaDetalle ajusteDetalle, out string errorMessage);
        bool TryRemoveAjusteBoletaPayment(Boletas boletaPayment, AjusteBoletaPago ajustePayment, out string errorMessage);
        bool TryApplyAjusteBoletaPayment(AjusteBoletaPago ajusteBoletaPago, Boletas boletaPayment, out string errorMessage);
        bool TryActiveAjusteBoleta(AjusteBoleta ajusteBoleta, out string errorMessage);
        bool TryUpdateAjusteBoletaPago(AjusteBoletaPago ajusteBoletaPago, Boletas boletaPayment, decimal monto, out string errorMessage);
        void TryCloseAjusteBoleta(List<AjusteBoletaPago> ajusteBoletaPagos);
        void TryRectiveAdjustment(Boletas boleta);
        List<AjusteBoletaDetalle> GetAvailableAjustmentDetails(List<AjusteBoletaDetalle> ajustmentDetails, int boletaId);
    }
}
