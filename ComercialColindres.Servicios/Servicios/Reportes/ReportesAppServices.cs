using System.Collections.Generic;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.Servicios.Clases;
using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Helpers;
using ComercialColindres.Datos.Entorno.Modelos;
using ComercialColindres.Datos.Entorno.Modelos.Reportes;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using System;
using ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.Humidity;
using ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.PendingInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BoletaWithOutInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BillsWithWeightsError;
using ComercialColindres.DTOs.RequestDTOs.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.HistoryOfInvoicceBalances;

namespace ComercialColindres.Servicios.Servicios
{
    public class ReportesAppServices : IReportesAppServices
    {
        private readonly ICacheAdapter _cacheAdapter;
        ComercialColindresContext _unidadDeTrabajo;

        public ReportesAppServices(ComercialColindresContext unidadDeTrabajo, ICacheAdapter cacheAdapter)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _cacheAdapter = cacheAdapter;
        }

        public RptPrestamosPorProveedorResumenDTO Get(GetPrestamosPorProveedor request)
        {
            var datosEncabezado = ReportesDataService.ReportePrestamosPorProveedorEncabezado(_unidadDeTrabajo, request.FechaInicio, request.FechaFinal, request.FiltrarPorFechas, request.ProveedorId, request.SucursalId);
            var datosDetalle = ReportesDataService.ReportePrestamosPorProveedorDetalle(_unidadDeTrabajo, request.FechaInicio, request.FechaFinal, request.FiltrarPorFechas, request.ProveedorId, request.SucursalId);

            var encabezado = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptPrestamosPorProveedorEncabezado>, IEnumerable<RptPrestamosPorProveedorEncabezadoDTO>>(datosEncabezado);
            var detalle = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptPrestamosPorProveedorDetalle>, IEnumerable<RptPrestamosPorProveedorDetalleDTO>>(datosDetalle);

            return new RptPrestamosPorProveedorResumenDTO
            {
                EncabezadoPrestamos = encabezado.ToList(),
                AbonosPorBoletas = detalle.ToList()
            };
        }
        
        public RptGasCreditoDetalleResumenDTO Get(GetGasCreditoDetalle request)
        {
            var datosEncabezado = ReportesDataService.ReporteGasCreditoEncabezado(_unidadDeTrabajo, request.GasCreditoId);
            var datosOrdenesOperativas = ReportesDataService.ReporteOrdenesCombustibleOperativo(_unidadDeTrabajo, request.GasCreditoId);
            var datosOrdenesPersonales = ReportesDataService.ReporteOrdenesCombustiblePersonales(_unidadDeTrabajo, request.GasCreditoId);

            var encabezado = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptGasCreditoEncabezado>, IEnumerable<RptGasCreditoEncabezadoDTO>>(datosEncabezado);
            var ordenesOperativas = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptOrdenesCombustibleOperativo>, IEnumerable<RptOrdenesCombustibleOperativoDTO>>(datosOrdenesOperativas);
            var ordenesPersonales = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptOrdenesCombustiblePersonales>, IEnumerable<RptOrdenesCombustiblePersonalesDTO>>(datosOrdenesPersonales);

            return new RptGasCreditoDetalleResumenDTO
            {
                GasCreditoEncabezado = encabezado.ToList(),
                OrdenesCombustibleOperativo = ordenesOperativas.ToList(),
                OrdenesCombustiblePersonales = ordenesPersonales.ToList()
            };
        }

        public RptCompraProductoBiomasaResumenDto Get(GetCompraProductoResumen request)
        {
            var datosReporte = ReportesDataService.ReporteCompraProductoBiomasaResumen(_unidadDeTrabajo, request.FechaInicio, request.FechaFinal);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptCompraProductoBiomasaResumen>, IEnumerable<CompraProductoBiomasaResumenDato>>(datosReporte);

            return new RptCompraProductoBiomasaResumenDto
            {
                CompraBiosaResumen = retorno.OrderByDescending(o => o.TotalCompra).ToList()
            };
        }

        public RptCompraProductoBiomasaDetalleDto Get(GetCompraProductoDetalle request)
        {
            var datosReporte = ReportesDataService.ReporteCompraProductoBiomasaDetalle(_unidadDeTrabajo, request.FechaInicio, request.FechaFinal);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptCompraProductoBiomasaDetalle>, IEnumerable<CompraProductoBiomasaDetalleDato>>(datosReporte);

            return new RptCompraProductoBiomasaDetalleDto
            {
                CompraBiomasaDetalle = retorno.OrderBy(o => o.FechaIngreso).ToList()
            };
        }

        public List<ReporteBoletaDeduccionesPrestamoDTO> Get(GetBoletaDeduccionesPrestamo request)
        {
            var datosReporte = ReportesDataService.ReporteBoletaDeduccionesPrestamo(_unidadDeTrabajo, request.PrestamoId);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ReporteBoletaDeduccionesPrestamo>, IEnumerable<ReporteBoletaDeduccionesPrestamoDTO>>(datosReporte);
            return retorno.ToList();
        }

        public List<ReporteDescargadoresDTO> Get(GetPreviewPagoDescargadores request)
        {
            var datosReporte = ReportesDataService.ReportePreviewPagoDescargadores(_unidadDeTrabajo, request.PagoDescargadoresId);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ReporteDescargadores>, IEnumerable<ReporteDescargadoresDTO>>(datosReporte);
            return retorno.ToList();
        }

        public ReportePagoDescargadoresResumen Get(GetFacturacionPagoDescargadores request)
        {
            var datosDescargas = ReportesDataService.ReportePreviewPagoDescargadores(_unidadDeTrabajo, request.PagoDescargadoresId);
            var datosPagoDescargas = ReportesDataService.ReporteFacturacionPagoDescargadores(_unidadDeTrabajo, request.PagoDescargadoresId);

            var descargas = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ReporteDescargadores>, IEnumerable<ReporteDescargadoresDTO>>(datosDescargas);
            var pagoDescargas = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ReportePagoDescargadores>, IEnumerable<ReportePagoDescargadoresDTO>>(datosPagoDescargas);

            return new ReportePagoDescargadoresResumen
            {
                Descargas = descargas.ToList(),
                PagoDescargadores = pagoDescargas.ToList()
            };
        }

        public List<ReporteFacturacionDTO> Get(GetFacturacion request)
        {
            var datosReporte = ReportesDataService.ReporteFacturacion(_unidadDeTrabajo, request.FacturacionId);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<ReporteFacturacion>, IEnumerable<ReporteFacturacionDTO>>(datosReporte);
            return retorno.ToList();
        }

        public RptHistorialBoletasDTO Get(GetHistorialBoletasPorProveedor request)
        {
            var datosReporte = ReportesDataService.ReporteHistorialBoletasPorProveedor(_unidadDeTrabajo, request.FechaInicio, request.FechaFinal, request.FiltrarPorFechas, request.ProveedorId);
            var historialBoletas = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptHistorialBoletas>, IEnumerable<RptHistorialDatoBoletasDTO>>(datosReporte);

            return new RptHistorialBoletasDTO
            {
                HistorialBoletas = historialBoletas.ToList()
            };
        }

        public RptPrestamosPendientesDTO Get(GetPrestamosPendientes request)
        {
            var datosReporte = ReportesDataService.ReportePrestamosPendientes(_unidadDeTrabajo);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptPrestamosPendientes>, IEnumerable<RptDatosPrestamosPendientes>>(datosReporte);

            return new RptPrestamosPendientesDTO
            {
                ListaPrestamosPendientes = retorno.OrderByDescending(o => o.SaldoPendiente).ToList()
            };
        }

        public RptBoletaPaymentPendingResumenDto GetBoletaPaymentPending(GetBoletaPaymentPending request)
        {
            var datosReporte = ReportesDataService.ReportBoletaPaymentPending(_unidadDeTrabajo, request.ProveedorId, request.BoletasId, request.IsPartialPayment);
            var retorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptBoletaPaymentPending>, IEnumerable<RptBoletaPaymentPendingDto>>(datosReporte);

            return new RptBoletaPaymentPendingResumenDto
            {
                BoletasPending = retorno.OrderByDescending(o => o.TotalPagar).ToList()
            };
        }

        public List<RptHumidityPendingPaymentDto> GetHumidityPendingPayment(GetHumidityPendingPayment request)
        {
            var datosReporte = ReportesDataService.ReportHumidityPendingPayment(_unidadDeTrabajo);
            var datosRetorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptHumidityPendingPayment>, IEnumerable<RptHumidityPendingPaymentDto>>(datosReporte);

            return datosRetorno.ToList();
        }

        public List<RptPendingInvoiceDto> GetPendingInvoice(GetPendingInvoice request)
        {
            var datosReporte = ReportesDataService.ReportPendingInvoice(_unidadDeTrabajo, request.FiltroBusqueda, request.PlantaId);
            var datosRetorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptPendingInvoice>, IEnumerable<RptPendingInvoiceDto>>(datosReporte);

            return datosRetorno.ToList();
        }

        public List<RptBoletaWithOutInvoiceDto> GetBoletasWithOutInvoice(GetBoletasWithOutInvoice request)
        {
            var datosReporte = ReportesDataService.ReportBoletasWithOutIvoice(_unidadDeTrabajo, request.FechaInicial, request.FechaFinal, request.FiltroBusqueda, request.FiltrarPorFechas, request.PlantaId);
            var datosRetorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptBoletaWithOutInvoice>, IEnumerable<RptBoletaWithOutInvoiceDto>>(datosReporte);

            return datosRetorno.ToList();
        }

        public List<RptBillWithWeightsErrorDto> GetBillsWithWeightsError(GetBillsWithWeightsError request)
        {
            var datosReporte = ReportesDataService.ReportBillsWithWeightsError(_unidadDeTrabajo, request.PlantaId);
            var datosRetorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptBillWithWeightsError>, IEnumerable<RptBillWithWeightsErrorDto>>(datosReporte);

            return datosRetorno.ToList();
        }

        public List<RptHistoryOfInvoiceBalancesDto> GetHistoryOfInvoiceBalances(GetHistoryOfInvoiceBalances request)
        {
            var datosReporte = ReportesDataService.ReportHistoryOfInvoiceBalances(_unidadDeTrabajo, request.FactoryId, request.EndDate);
            var datosRetorno = AutomapperTypeAdapter.ProyectarColeccionComo<IEnumerable<RptHistoryOfInvoiceBalances>, IEnumerable<RptHistoryOfInvoiceBalancesDto>>(datosReporte);

            return datosRetorno.ToList();
        }
    }
}
