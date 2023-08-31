using ComercialColindres.Datos.Entorno;
using ComercialColindres.Datos.Entorno.Modelos;
using ComercialColindres.Datos.Entorno.Modelos.Reportes;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BillsWithWeightsError;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BoletaWithOutInvoice;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.Humidity;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.PendingInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ComercialColindres.Datos.Helpers
{
    public class ReportesDataService
    {
        public static List<ReporteFacturacion> ReporteFacturacion(ComercialColindresContext unidadDeTrabajo, int FacturaId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@FacturaId";
            parametro1.Value = FacturaId;

            var parametros = new List<SqlParameter>
                {
                    parametro1
                };

            var datos =
                unidadDeTrabajo.Database.SqlQuery<ReporteFacturacion>("EXEC spReporteFacturacion @FacturaId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<ReporteDescargadores> ReportePreviewPagoDescargadores(ComercialColindresContext unidadDeTrabajo, int PagoDescargaId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@PagoDescargasId";
            parametro1.Value = PagoDescargaId;

            var parametros = new List<SqlParameter>
                {
                    parametro1
                };

            var datos =
                unidadDeTrabajo.Database.SqlQuery<ReporteDescargadores>("EXEC spObtenerDescargas @PagoDescargasId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<ReportePagoDescargadores> ReporteFacturacionPagoDescargadores(ComercialColindresContext unidadDeTrabajo, int PagoDescargaId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@PagoDescargasId";
            parametro1.Value = PagoDescargaId;

            var parametros = new List<SqlParameter>
                {
                    parametro1
                };

            var datos =
                unidadDeTrabajo.Database.SqlQuery<ReportePagoDescargadores>("EXEC spObtenerPagoDescargadores @PagoDescargasId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<ReporteBoletaDeduccionesPrestamo> ReporteBoletaDeduccionesPrestamo(ComercialColindresContext unidadTrabajo, int prestamoId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@PrestamoId";
            parametro1.Value = prestamoId;

            var parametros = new List<SqlParameter>
            {
                parametro1
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<ReporteBoletaDeduccionesPrestamo>("EXEC spBoletasDeduccionesPrestamo @PrestamoId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptPrestamosPorProveedorEncabezado> ReportePrestamosPorProveedorEncabezado(ComercialColindresContext unidadTrabajo, DateTime fechaInicio, 
                                                                                        DateTime fechaFinal, bool filtrarPorFechas, int proveedorId, int sucursalId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.DateTime;
            parametro1.ParameterName = "@FechaInicio";
            parametro1.Value = fechaInicio;

            var parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.DateTime;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            var parametro3 = new SqlParameter();
            parametro3.SqlDbType = SqlDbType.Bit;
            parametro3.ParameterName = "@FiltrarPorFechas";
            parametro3.Value = filtrarPorFechas;

            var parametro4 = new SqlParameter();
            parametro4.SqlDbType = SqlDbType.Int;
            parametro4.ParameterName = "@ProveedorId";
            parametro4.Value = proveedorId;

            var parametro5 = new SqlParameter();
            parametro5.SqlDbType = SqlDbType.Int;
            parametro5.ParameterName = "@SucursalId";
            parametro5.Value = sucursalId;

            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2, parametro3, parametro4, parametro5
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptPrestamosPorProveedorEncabezado>("EXEC Sp_ReportePrestamosEncabezadoPorProveedor @SucursalId, @FechaInicio, @FechaFinal, @FiltrarPorFechas, @ProveedorId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptCompraProductoBiomasaResumen> ReporteCompraProductoBiomasaResumen(ComercialColindresContext unidadTrabajo, DateTime fechaInicio, DateTime fechaFinal)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.DateTime;
            parametro1.ParameterName = "@FechaInicio";
            parametro1.Value = fechaInicio;

            var parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.DateTime;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptCompraProductoBiomasaResumen>("EXEC ObtenerCompraProductoBiomasaResumen @FechaInicio, @FechaFinal",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptCompraProductoBiomasaDetalle> ReporteCompraProductoBiomasaDetalle(ComercialColindresContext unidadTrabajo, DateTime fechaInicio, DateTime fechaFinal)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.DateTime;
            parametro1.ParameterName = "@FechaInicio";
            parametro1.Value = fechaInicio;

            var parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.DateTime;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptCompraProductoBiomasaDetalle>("EXEC ObtenerCompraProductoBiomasaDetalle @FechaInicio, @FechaFinal",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptPrestamosPorProveedorDetalle> ReportePrestamosPorProveedorDetalle(ComercialColindresContext unidadTrabajo, DateTime fechaInicio, 
                                                                        DateTime fechaFinal, bool filtrarPorFechas, int proveedorId, int sucursalId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.DateTime;
            parametro1.ParameterName = "@FechaInicio";
            parametro1.Value = fechaInicio;

            var parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.DateTime;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            var parametro3 = new SqlParameter();
            parametro3.SqlDbType = SqlDbType.Bit;
            parametro3.ParameterName = "@FiltrarPorFechas";
            parametro3.Value = filtrarPorFechas;

            var parametro4 = new SqlParameter();
            parametro4.SqlDbType = SqlDbType.Int;
            parametro4.ParameterName = "@ProveedorId";
            parametro4.Value = proveedorId;

            var parametro5 = new SqlParameter();
            parametro5.SqlDbType = SqlDbType.Int;
            parametro5.ParameterName = "@SucursalId";
            parametro5.Value = sucursalId;

            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2, parametro3, parametro4, parametro5
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptPrestamosPorProveedorDetalle>("EXEC Sp_ReportePrestamosDetallePorProveedor @SucursalId, @FechaInicio, @FechaFinal, @FiltrarPorFechas, @ProveedorId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptHistorialBoletas> ReporteHistorialBoletasPorProveedor(ComercialColindresContext unidadDeTrabajo, DateTime fechaInicio, DateTime fechaFinal, bool filtrarPorFechas, int proveedorId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.DateTime;
            parametro1.ParameterName = "@FechaInicio";
            parametro1.Value = fechaInicio;

            var parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.DateTime;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            var parametro3 = new SqlParameter();
            parametro3.SqlDbType = SqlDbType.Bit;
            parametro3.ParameterName = "@FiltrarPorFechas";
            parametro3.Value = filtrarPorFechas;

            var parametro4 = new SqlParameter();
            parametro4.SqlDbType = SqlDbType.Int;
            parametro4.ParameterName = "@ProveedorId";
            parametro4.Value = proveedorId;
            
            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2, parametro3, parametro4
            };

            var datos =
                unidadDeTrabajo.Database.SqlQuery<RptHistorialBoletas>("EXEC Sp_ReporteHistorialBoletasPorProveedor @FechaInicio, @FechaFinal, @FiltrarPorFechas, @ProveedorId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptGasCreditoEncabezado> ReporteGasCreditoEncabezado(ComercialColindresContext unidadTrabajo, int gasCreditoId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@GasCreditoId";
            parametro1.Value = gasCreditoId;

            var parametros = new List<SqlParameter>
            {
                parametro1
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptGasCreditoEncabezado>("EXEC spObtenerLineaGasCredito @GasCreditoId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptOrdenesCombustibleOperativo> ReporteOrdenesCombustibleOperativo(ComercialColindresContext unidadTrabajo, int gasCreditoId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@GasCreditoId";
            parametro1.Value = gasCreditoId;

            var parametros = new List<SqlParameter>
            {
                parametro1
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptOrdenesCombustibleOperativo>("EXEC spObtenerOrdenesCombustibleOperativo @GasCreditoId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptOrdenesCombustiblePersonales> ReporteOrdenesCombustiblePersonales(ComercialColindresContext unidadTrabajo, int gasCreditoId)
        {
            var parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@GasCreditoId";
            parametro1.Value = gasCreditoId;

            var parametros = new List<SqlParameter>
            {
                parametro1
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptOrdenesCombustiblePersonales>("EXEC spObtenerOrdenesCombustiblePersonales @GasCreditoId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptPrestamosPendientes> ReportePrestamosPendientes(ComercialColindresContext unidadTrabajo)
        {
            var datos =
                unidadTrabajo.Database.SqlQuery<RptPrestamosPendientes>("EXEC spObtenerPrestamosPendientes");
            return datos.ToList();
        }

        public static List<RptBoletaPaymentPending> ReportBoletaPaymentPending(ComercialColindresContext unidadTrabajo, int proveedorId, List<int> boletasId, bool isPartialPayment = false)
        {
            DataTable boletasTable = new DataTable("BoletasId");
            boletasTable.Columns.Add("BoletaId", typeof(int));

            if (isPartialPayment == true && boletasId != null)
            {
                foreach (int item in boletasId)
                {
                    boletasTable.Rows.Add(item);
                }
            }

            SqlParameter parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@ProveedorId";
            parametro1.Value = proveedorId;

            SqlParameter parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.Bit;
            parametro2.ParameterName = "@IsPartialPayment";
            parametro2.Value = isPartialPayment;

            SqlParameter parametro3 = new SqlParameter();
            parametro3.SqlDbType = SqlDbType.Structured;
            parametro3.ParameterName = "@BoletasId";
            parametro3.TypeName = "dbo.tblBoletas";
            parametro3.Value = boletasTable;

            var parametros = new List<SqlParameter>
            {
                parametro1, parametro2, parametro3
            };

            var datos =
                unidadTrabajo.Database.SqlQuery<RptBoletaPaymentPending>("EXEC spGetBoletaPaymentPending @ProveedorId, @IsPartialPayment, @BoletasId",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptHumidityPendingPayment> ReportHumidityPendingPayment(ComercialColindresContext unidadTrabajo)
        {
            var parametros = new List<SqlParameter>();

            var datos =
                unidadTrabajo.Database.SqlQuery<RptHumidityPendingPayment>("EXEC spGetHumidityPendingPayment",
                parametros.ToArray());
            return datos.ToList();
        }

        public static List<RptPendingInvoice> ReportPendingInvoice(ComercialColindresContext uow, string filterValue, int plantaId)
        {
            SqlParameter parametro1 = new SqlParameter
            {
                SqlDbType = SqlDbType.VarChar,
                ParameterName = "@FiltroBusqueda",
                Value = filterValue
            };

            SqlParameter parametro2 = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@PlantaId",
                Value = plantaId
            };

            var parametros = new List<SqlParameter> { parametro1, parametro2 };

            var datos = uow.Database.SqlQuery<RptPendingInvoice>("spGetPendingInvoice @FiltroBusqueda, @PlantaId",
                parametros.ToArray());

            return datos.ToList();
        }

        public static List<RptBoletaWithOutInvoice> ReportBoletasWithOutIvoice(ComercialColindresContext uow, DateTime fechaInicio, DateTime fechaFinal, string filterValue, bool filtrarPorFechas, int plantaId)
        {
            SqlParameter parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Date;
            parametro1.ParameterName = "@FechaInicial";
            parametro1.Value = fechaInicio;

            SqlParameter parametro2 = new SqlParameter();
            parametro2.SqlDbType = SqlDbType.Date;
            parametro2.ParameterName = "@FechaFinal";
            parametro2.Value = fechaFinal;

            SqlParameter parametro3 = new SqlParameter();
            parametro3.SqlDbType = SqlDbType.VarChar;
            parametro3.ParameterName = "@FiltroBusqueda";
            parametro3.Value = filterValue;

            SqlParameter parametro4 = new SqlParameter();
            parametro4.SqlDbType = SqlDbType.Bit;
            parametro4.ParameterName = "@FiltrarPorFechas";
            parametro4.Value = filtrarPorFechas;

            SqlParameter parametro5 = new SqlParameter();
            parametro5.SqlDbType = SqlDbType.Int;
            parametro5.ParameterName = "@PlantaId";
            parametro5.Value = plantaId;

            var parametros = new List<SqlParameter> { parametro1, parametro2, parametro3, parametro4, parametro5 };

            var datos = uow.Database.SqlQuery<RptBoletaWithOutInvoice>("spGetBoletasWithOutInvoice @FechaInicial, @FechaFinal, @FiltroBusqueda, @FiltrarPorFechas, @PlantaId",
                parametros.ToArray());

            return datos.ToList();
        }

        public static List<RptBillWithWeightsError> ReportBillsWithWeightsError(ComercialColindresContext uow, int plantaId)
        {
            SqlParameter parametro1 = new SqlParameter();
            parametro1.SqlDbType = SqlDbType.Int;
            parametro1.ParameterName = "@PlantaId";
            parametro1.Value = plantaId;

            var parametros = new List<SqlParameter> { parametro1 };

            var datos = uow.Database.SqlQuery<RptBillWithWeightsError>("spGetBillsWithWeightsError @PlantaId",
                parametros.ToArray());

            return datos.ToList();
        }

        public static List<RptHistoryOfInvoiceBalances> ReportHistoryOfInvoiceBalances(ComercialColindresContext uow, int factoryId, DateTime endDate)
        {
            SqlParameter parametro1 = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@FactoryId",
                Value = factoryId
            };

            SqlParameter parametro2 = new SqlParameter
            {
                SqlDbType = SqlDbType.Date,
                ParameterName = "@EndDate",
                Value = endDate
            };

            var parametros = new List<SqlParameter> { parametro1, parametro2 };

            var datos = uow.Database.SqlQuery<RptHistoryOfInvoiceBalances>("spGetHistoryOfInvoiceBalances @FactoryId, @EndDate",
                parametros.ToArray());

            return datos.ToList();
        }
    }
}
