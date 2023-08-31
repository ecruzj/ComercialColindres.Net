using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities;
using ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Servicios
{
    public interface IReportesAppServices
    {
        List<ReporteFacturacionDTO> Get(GetFacturacion request);
        ReportePagoDescargadoresResumen Get(GetFacturacionPagoDescargadores request);
        List<ReporteDescargadoresDTO> Get(GetPreviewPagoDescargadores request);
        List<ReporteBoletaDeduccionesPrestamoDTO> Get(GetBoletaDeduccionesPrestamo request);
        RptGasCreditoDetalleResumenDTO Get(GetGasCreditoDetalle request);
        RptPrestamosPorProveedorResumenDTO Get(GetPrestamosPorProveedor request);
        RptHistorialBoletasDTO Get(GetHistorialBoletasPorProveedor request);
        RptPrestamosPendientesDTO Get(GetPrestamosPendientes request);
        RptCompraProductoBiomasaResumenDto Get(GetCompraProductoResumen request);
        RptCompraProductoBiomasaDetalleDto Get(GetCompraProductoDetalle request);
        RptBoletaPaymentPendingResumenDto GetBoletaPaymentPending(GetBoletaPaymentPending request);
        List<RptHumidityPendingPaymentDto> GetHumidityPendingPayment(GetHumidityPendingPayment request);
        List<RptPendingInvoiceDto> GetPendingInvoice(GetPendingInvoice request);
        List<RptBoletaWithOutInvoiceDto> GetBoletasWithOutInvoice(GetBoletasWithOutInvoice request);
        List<RptBillWithWeightsErrorDto> GetBillsWithWeightsError(GetBillsWithWeightsError request);
        List<RptHistoryOfInvoiceBalancesDto> GetHistoryOfInvoiceBalances(GetHistoryOfInvoiceBalances request);
    }
}
