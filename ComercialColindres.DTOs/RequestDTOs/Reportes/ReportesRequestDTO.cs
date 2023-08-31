using ComercialColindres.DTOs.Clases;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities;
using ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;

namespace ComercialColindres.DTOs.RequestDTOs.Reportes
{
    [Route("/reportes/facturaciones-facturacion", "GET")]
    public class GetFacturacion : IReturn<List<ReporteFacturacionDTO>>
    {
        public int FacturacionId { get; set; }
    }

    [Route("/reportes/facturaciones-pago-descargadores", "GET")]
    public class GetFacturacionPagoDescargadores : IReturn<ReportePagoDescargadoresResumen>
    {
        public int PagoDescargadoresId { get; set; }
    }

    [Route("/reportes/pago-descargadores", "GET")]
    public class GetPreviewPagoDescargadores : IReturn<List<ReporteDescargadoresDTO>>
    {
        public int PagoDescargadoresId { get; set; }
    }

    [Route("/reportes/gas-creditos", "GET")]
    public class GetGasCreditoDetalle : IReturn<RptGasCreditoDetalleResumenDTO>
    {
        public int GasCreditoId { get; set; }
    }

    [Route("/reportes/prestamo-boleta-deducciones", "GET")]
    public class GetBoletaDeduccionesPrestamo : IReturn<List<ReporteBoletaDeduccionesPrestamoDTO>>
    {
        public int PrestamoId { get; set; }
    }

    [Route("/reportes/prestamos/historial-por-proveedor", "GET")]
    public class GetPrestamosPorProveedor : IReturn<RptPrestamosPorProveedorResumenDTO>
    {
        public int SucursalId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public bool FiltrarPorFechas { get; set; }
        public int ProveedorId { get; set; }
    }

    [Route("/reportes/boletas/historial-por-proveedor", "GET")]
    public class GetHistorialBoletasPorProveedor : IReturn<RptHistorialBoletasDTO>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public bool FiltrarPorFechas { get; set; }
        public int ProveedorId { get; set; }
    }

    [Route("/reportes/prestamos/prestamos-pendientes-de-cobrar", "GET")]
    public class GetPrestamosPendientes : IReturn<RptPrestamosPendientesDTO>
    {
    }

    [Route("/reportes/compras/compra-producto-resumen", "GET")]
    public class GetCompraProductoResumen : IReturn<RptCompraProductoBiomasaResumenDto>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }

    [Route("/reportes/compras/compra-producto-detalle", "GET")]
    public class GetCompraProductoDetalle : IReturn<RptCompraProductoBiomasaDetalleDto>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }

    [Route("/reportes/boletas/pending-payment", "GET")]
    public class GetBoletaPaymentPending : RequestBase, IReturn<RptBoletaPaymentPendingResumenDto>
    {
        public int ProveedorId { get; set; }
        public bool IsPartialPayment { get; set; }
        public List<int> BoletasId { get; set; }
    }

    [Route("/reportes/humidity/pending-payment", "GET")]
    public class GetHumidityPendingPayment : RequestBase, IReturn<List<RptHumidityPendingPaymentDto>>
    {
    }

    [Route("/reportes/pending-invoice", "GET")]
    public class GetPendingInvoice : RequestBase, IReturn<List<RptPendingInvoiceDto>>
    {
        public string FiltroBusqueda { get; set; }
        public int PlantaId { get; set; }
    }

    [Route("/reportes/boletas-with-out-invoice", "GET")]
    public class GetBoletasWithOutInvoice : RequestBase, IReturn<List<RptBoletaWithOutInvoiceDto>>
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string FiltroBusqueda { get; set; }
        public bool FiltrarPorFechas { get; set; }
        public int PlantaId { get; set; }
    }

    [Route("/reportes/bills-with-weight-error", "GET")]
    public class GetBillsWithWeightsError : RequestBase, IReturn<List<RptBillWithWeightsErrorDto>>
    {
        public int PlantaId { get; set; }
    }

    [Route("/reportes/history-of-invoice-balances", "GET")]
    public class GetHistoryOfInvoiceBalances : RequestBase, IReturn<List<RptHistoryOfInvoiceBalancesDto>>
    {
        public int FactoryId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
