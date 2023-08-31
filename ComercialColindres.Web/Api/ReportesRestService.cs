using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Servicios.Servicios;
using ServiceStack.ServiceHost;

namespace ComercialColindres.Web.Api
{
    public class ReportesRestService : IService
    {
        private readonly IReportesAppServices _reporteAppService;

        public ReportesRestService(IReportesAppServices reporteAppService)
        {
            _reporteAppService = reporteAppService;
        }

        public object Get(GetFacturacion request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetFacturacionPagoDescargadores request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetPreviewPagoDescargadores request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetGasCreditoDetalle request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetBoletaDeduccionesPrestamo request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetPrestamosPorProveedor request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetHistorialBoletasPorProveedor request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetPrestamosPendientes request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetCompraProductoResumen request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetCompraProductoDetalle request)
        {
            return _reporteAppService.Get(request);
        }

        public object Get(GetBoletaPaymentPending request)
        {
            return _reporteAppService.GetBoletaPaymentPending(request);
        }

        public object Get(GetHumidityPendingPayment request)
        {
            return _reporteAppService.GetHumidityPendingPayment(request);
        }

        public object Get(GetPendingInvoice request)
        {
            return _reporteAppService.GetPendingInvoice(request);
        }

        public object Get(GetBoletasWithOutInvoice request)
        {
            return _reporteAppService.GetBoletasWithOutInvoice(request);
        }

        public object Get(GetBillsWithWeightsError request)
        {
            return _reporteAppService.GetBillsWithWeightsError(request);
        }

        public object Get(GetHistoryOfInvoiceBalances request)
        {
            return _reporteAppService.GetHistoryOfInvoiceBalances(request);
        }
    }
}