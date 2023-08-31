using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Entorno.Modelos;
using ComercialColindres.DTOs.RequestDTOs.Bancos;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.DTOs.RequestDTOs.EquiposCategorias;
using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.DTOs.RequestDTOs.Opciones;
using ComercialColindres.DTOs.RequestDTOs.Sucursales;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.DTOs.RequestDTOs.ClientePlantas;
using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.RequestDTOs.CategoriaProductos;
using ComercialColindres.DTOs.RequestDTOs.PrecioProductos;
using ComercialColindres.DTOs.RequestDTOs.PrecioDescargas;
using ComercialColindres.DTOs.RequestDTOs.BoletaDetalles;
using ComercialColindres.DTOs.RequestDTOs.Gasolineras;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.RequestDTOs.BoletaCierres;
using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.DTOs.RequestDTOs.PagoPrestamos;
using System.Linq;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargaDetalles;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditoPagos;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalle;
using ComercialColindres.DTOs.RequestDTOs.Recibos;
using ComercialColindres.DTOs.RequestDTOs.FacturasCategorias;
using ComercialColindres.DTOs.RequestDTOs.Reportes;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieraTipos;
using ComercialColindres.DTOs.RequestDTOs.CuentasFinancieras;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.LineasCreditoDeducciones;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProductoDetalle;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraDetalleBoleta;
using ComercialColindres.DTOs.RequestDTOs.MaestroBiomasa;
using ComercialColindres.DTOs.RequestDTOs.BoletaOtrasDeducciones;
using AutoMapper;
using ComercialColindres.Datos.Entorno.Modelos.Reportes;
using ComercialColindres.DTOs.RequestDTOs.DescargasPorAdelantado;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletaPaymentPending;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.DTOs.RequestDTOs.BoletaPagoHumedad;
using ComercialColindres.DTOs.RequestDTOs.BoletaHumedadAsignacion;
using ComercialColindres.DTOs.RequestDTOs.Reportes.Humidity.PendingHumidities;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.Humidity;
using ComercialColindres.DTOs.RequestDTOs.BoletaDeduccionManual;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using ComercialColindres.DTOs.RequestDTOs.NotasCredito;
using ComercialColindres.DTOs.RequestDTOs.Bonificaciones;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.PendingInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.PendingInvoice;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BoletaWithOutInvoice;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BoletasWithOutInvoice;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.BillsWithWeightsError;
using ComercialColindres.DTOs.RequestDTOs.Reportes.BillsWithWeightsError;
using ComercialColindres.DTOs.RequestDTOs.AjusteTipos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaPagos;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletaDetalle;
using ComercialColindres.Datos.Entorno.Modelos.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.DTOs.RequestDTOs.Reportes.HistoryOfInvoicceBalances;
using ComercialColindres.DTOs.RequestDTOs.FuelOrderManualPayments;

namespace ComercialColindres.Web.MapeosDTOs
{
    public static class InitializeAutomap
    {
        public static void InitializarAutomap()
        {
            Mapper.Initialize(map =>
            {

                map.CreateMap<ClientePlantas, ClientePlantasDTO>();
                map.CreateMap<Sucursales, SucursalesDTO>();
                map.CreateMap<Opciones, OpcionesDTO>();
                map.CreateMap<Proveedores, ProveedoresDTO>();
                map.CreateMap<Bancos, BancosDTO>();                
                map.CreateMap<PrecioProductos, PrecioProductosDTO>();
                map.CreateMap<PrecioDescargas, PrecioDescargasDTO>();

                var mapCategoriaProductos = map.CreateMap<CategoriaProductos, CategoriaProductosDTO>();
                mapCategoriaProductos.ForMember(dto => dto.DescripcionBiomasa, mce => mce.MapFrom(exp => exp.MaestroBiomasa.Descripcion));

                var mapCuadrillas = map.CreateMap<Cuadrillas, CuadrillasDTO>();
                mapCuadrillas.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.ClientePlanta.NombrePlanta));

                var mapBoletas = map.CreateMap<Boletas, BoletasDTO>();
                mapBoletas.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.Proveedor.Nombre));
                mapBoletas.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.ClientePlanta.NombrePlanta));
                mapBoletas.ForMember(dto => dto.DescripcionTipoProducto, mce => mce.MapFrom(exp => exp.CategoriaProducto.Descripcion));
                mapBoletas.ForMember(dto => dto.DescripcionTipoEquipo, mce => mce.MapFrom(exp => exp.ObtenerDescripcionEquipo()));
                mapBoletas.ForMember(dto => dto.NombreDescargador, mce => mce.MapFrom(exp => exp.Descargador.GetCuadrillaName()));
                mapBoletas.ForMember(dto => dto.DescargaId, mce => mce.MapFrom(exp => exp.Descargador.DescargadaId));
                mapBoletas.ForMember(dto => dto.PrecioDescarga, mce => mce.MapFrom(exp => exp.Descargador.PrecioDescarga));
                mapBoletas.ForMember(dto => dto.EquipoCategoriaId, mce => mce.MapFrom(exp => exp.ObtenerCategoriaEquipo()));
                mapBoletas.ForMember(dto => dto.TotalFacturaCompra, mce => mce.MapFrom(exp => exp.ObtenerTotalFacturaCompra()));
                mapBoletas.ForMember(dto => dto.TotalFacturaVenta, mce => mce.MapFrom(exp => exp.ObtenerTotalFacturaVenta()));
                mapBoletas.ForMember(dto => dto.TotalAPagar, mce => mce.MapFrom(exp => exp.ObtenerTotalAPagar()));
                mapBoletas.ForMember(dto => dto.TotalDeduccion, mce => mce.MapFrom(exp => exp.ObtenerTotalDeduccion()));
                mapBoletas.ForMember(dto => dto.TotalDeuda, mce => mce.MapFrom(exp => exp.Proveedor.GetPendingLoan()));
                mapBoletas.ForMember(dto => dto.TotalBoletaOtrasDeducciones, mce => mce.MapFrom(exp => exp.ObtenerTotalOtrasDeduccionesNegativas()));
                mapBoletas.ForMember(dto => dto.TotalOtrosIngresosBoleta, mce => mce.MapFrom(exp => exp.ObtenerTotalOtrasIngresosBoleta()));
                mapBoletas.ForMember(dto => dto.TotalToneladas, mce => mce.MapFrom(exp => exp.GetWeightWithoutBonus()));
                mapBoletas.ForMember(dto => dto.DiasAntiguos, mce => mce.MapFrom(exp => (exp.ObtenerAntiguedadBoleta())));
                mapBoletas.ForMember(dto => dto.ClientHasLoan, mce => mce.MapFrom(exp => exp.ClientHasLoan()));
                mapBoletas.ForMember(dto => dto.ClientHasOutStandingHumidity, mce => mce.MapFrom(exp => exp.ClientHasOutStandingHumidiy()));
                mapBoletas.ForMember(dto => dto.ApplieToForceClosing, mce => mce.MapFrom(exp => exp.AppliesToForceClosing()));
                mapBoletas.ForMember(dto => dto.ApplieToOpen, mce => mce.MapFrom(exp => exp.IsClose()));
                mapBoletas.ForMember(dto => dto.IsAssignedInvoice, mce => mce.MapFrom(exp => exp.IsAssignedToInvoice()));
                mapBoletas.ForMember(dto => dto.FacturaNo, mce => mce.MapFrom(exp => exp.GetInoviceNo()));
                mapBoletas.ForMember(dto => dto.HasBonus, mce => mce.MapFrom(exp => exp.BoletaHasBonus()));
                mapBoletas.ForMember(dto => dto.IsBonusEnable, mce => mce.MapFrom(exp => exp.BoletaHasBonus()));
                mapBoletas.ForMember(dto => dto.IsShippingNumberRequired, mce => mce.MapFrom(exp => exp.IsShippingNumberRequired()));
                mapBoletas.ForMember(dto => dto.IsDescargaAdelanto, mce => mce.MapFrom(exp => exp.IsDescargaAdelanto()));
                mapBoletas.ForMember(dto => dto.IsHorizontalImg, mce => mce.MapFrom(exp => exp.IsHorizontalImg()));
                mapBoletas.ForMember(dto => dto.ClientHasPendingAdjustments, mce => mce.MapFrom(exp => exp.ClientHasPendingAdjustments()));
                mapBoletas.ForMember(dto => dto.HasFuelOrderPending, mce => mce.MapFrom(exp => exp.HasFuelOrderPending()));

                var mapCuentasBancarias = map.CreateMap<CuentasBancarias, CuentasBancariasDTO>();
                mapCuentasBancarias.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.Banco.Descripcion));

                var mapEquipos = map.CreateMap<Equipos, EquiposDTO>();
                mapEquipos.ForMember(dto => dto.DescripcionCategoria, mce => mce.MapFrom(exp => exp.EquiposCategoria.Descripcion));

                map.CreateMap<EquiposCategorias, EquiposCategoriasDTO>();
                map.CreateMap<Conductores, ConductoresDTO>();

                var mapUsuariosOpciones = map.CreateMap<UsuariosOpciones, UsuariosOpcionesDTO>();
                mapUsuariosOpciones.ForMember(dto => dto.NombreOpcion, mce => mce.MapFrom(exp => exp.Opcion.Nombre));
                mapUsuariosOpciones.ForMember(dto => dto.NombreSucursal, mce => mce.MapFrom(exp => exp.Sucursal.Nombre));
                mapUsuariosOpciones.ForMember(dto => dto.TipoPropiedad, mce => mce.MapFrom(exp => exp.Opcion.TipoPropiedad));
                mapUsuariosOpciones.ForMember(dto => dto.TipoAcceso, mce => mce.MapFrom(exp => exp.Opcion.TipoAcceso));

                var mapUsuario = map.CreateMap<Usuarios, UsuariosDTO>();
                mapUsuario.ForMember(dto => dto.UsuariosSucursalesAsignadas, mce => mce.MapFrom(exp => exp.ObtenerSucursalesAsignadas()));
                mapUsuario.ForMember(dto => dto.Clave, mce => mce.MapFrom(exp => exp.ObtenerClave()));

                map.CreateMap<UsuariosSucursalesAsignadas, UsuariosSucursalesAsignadasDTO>();

                var mapBoletaDetalles = map.CreateMap<BoletaDetalles, BoletaDetallesDTO>();
                mapBoletaDetalles.ForMember(dto => dto.DescripcionDeduccion, mce => mce.MapFrom(exp => exp.Deduccion.Descripcion));

                map.CreateMap<Gasolineras, GasolinerasDTO>();

                var mapOrdenesCombustible = map.CreateMap<OrdenesCombustible, OrdenesCombustibleDTO>();
                mapOrdenesCombustible.ForMember(dto => dto.NombreGasolinera, mce => mce.MapFrom(exp => exp.GasolineraCredito.Gasolinera.Descripcion));
                mapOrdenesCombustible.ForMember(dto => dto.CodigoCreditoCombustible, mce => mce.MapFrom(exp => exp.GasolineraCredito.CodigoGasCredito));
                mapOrdenesCombustible.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapOrdenesCombustible.ForMember(dto => dto.EstadoBoleta, mce => mce.MapFrom(exp => exp.Boleta.Estado));
                mapOrdenesCombustible.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.GetVendorName()));
                mapOrdenesCombustible.ForMember(dto => dto.FuelOrderSpecification, mce => mce.MapFrom(exp => exp.GetFuelOrderSpecification()));
                mapOrdenesCombustible.ForMember(dto => dto.Payments, mce => mce.MapFrom(exp => exp.GetPayments()));
                mapOrdenesCombustible.ForMember(dto => dto.OutStandingBalance, mce => mce.MapFrom(exp => exp.GetOutStandingBalance()));
                mapOrdenesCombustible.ForMember(dto => dto.IsClosed, mce => mce.MapFrom(exp => exp.IsFuelOrderClosed()));

                var mapBoletaCierres = map.CreateMap<BoletaCierres, BoletaCierresDTO>();
                mapBoletaCierres.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapBoletaCierres.ForMember(dto => dto.BancoId, mce => mce.MapFrom(exp => exp.LineasCredito.CuentasFinanciera.Banco.BancoId));
                mapBoletaCierres.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.LineasCredito.CuentasFinanciera.Banco.Descripcion));
                mapBoletaCierres.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.LineasCredito.CodigoLineaCredito + " - " + exp.LineasCredito.CuentasFinanciera.CuentaNo));
                mapBoletaCierres.ForMember(dto => dto.PuedeEditarCreditoDeduccion, mce => mce.MapFrom(exp => exp.LineasCredito.Estado == "ACTIVO"));

                var mapPrestamos = map.CreateMap<Prestamos, PrestamosDTO>();
                mapPrestamos.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.Proveedor.Nombre));
                mapPrestamos.ForMember(dto => dto.NombreSucursal, mce => mce.MapFrom(exp => exp.Sucursal.Nombre));
                mapPrestamos.ForMember(dto => dto.TotalACobrar, mce => mce.MapFrom(exp => exp.ObtenerTotalACobrar()));
                mapPrestamos.ForMember(dto => dto.TotalAbono, mce => mce.MapFrom(exp => exp.ObtenerTotalAbono()));
                mapPrestamos.ForMember(dto => dto.SaldoPendiente, mce => mce.MapFrom(exp => exp.ObtenerSaldoPendiente()));
                mapPrestamos.ForMember(dto => dto.DiasTranscurridos, mce => mce.MapFrom(exp => exp.GetIntervalDays()));
                mapPrestamos.ForMember(dto => dto.Intereses, mce => mce.MapFrom(exp => exp.GetInterests()));

                var mapPrestamosTransferencias = map.CreateMap<PrestamosTransferencias, PrestamosTransferenciasDTO>();
                mapPrestamosTransferencias.ForMember(dto => dto.BancoId, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.BancoId));
                mapPrestamosTransferencias.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.Descripcion));
                mapPrestamosTransferencias.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.LineaCredito.CodigoLineaCredito + " - " + exp.LineaCredito.CuentasFinanciera.CuentaNo));
                mapPrestamosTransferencias.ForMember(dto => dto.PuedeEditarCreditoDeduccion, mce => mce.MapFrom(exp => exp.LineaCredito.Estado == "ACTIVO"));

                var mapPagoPrestamos = map.CreateMap<PagoPrestamos, PagoPrestamosDTO>();
                mapPagoPrestamos.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapPagoPrestamos.ForMember(dto => dto.PuedeEditarAbonoPorBoleta, mce => mce.MapFrom(exp => exp.Boleta.Estado == "ACTIVO"));
                mapPagoPrestamos.ForMember(dto => dto.PlantaDestino, mce => mce.MapFrom(exp => exp.Boleta.ClientePlanta.NombrePlanta));
                mapPagoPrestamos.ForMember(dto => dto.CodigoPrestamo, mce => mce.MapFrom(exp => exp.Prestamo.CodigoPrestamo));
                mapPagoPrestamos.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.Banco.Descripcion));
                mapPagoPrestamos.ForMember(dto => dto.NoDocumento, mce => mce.MapFrom(exp => exp.GetNoDocument()));

                var mapDescargadores = map.CreateMap<Descargadores, DescargadoresDTO>();
                mapDescargadores.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapDescargadores.ForMember(dto => dto.EstadoBoleta, mce => mce.MapFrom(exp => exp.Boleta.Estado));
                mapDescargadores.ForMember(dto => dto.CodigoPagoDescarga, mce => mce.MapFrom(exp => exp.PagoDescargador.CodigoPagoDescarga));
                mapDescargadores.ForMember(dto => dto.EncargadoCuadrilla, mce => mce.MapFrom(exp => exp.Cuadrilla.NombreEncargado));
                mapDescargadores.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.Boleta.ClientePlanta.NombrePlanta));
                mapDescargadores.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.Boleta.Proveedor.Nombre));
                mapDescargadores.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.Boleta.NumeroEnvio));
                mapDescargadores.ForMember(dto => dto.PlacaEquipo, mce => mce.MapFrom(exp => exp.Boleta.PlacaEquipo));
                mapDescargadores.ForMember(dto => dto.Motorista, mce => mce.MapFrom(exp => exp.Boleta.Motorista));
                mapDescargadores.ForMember(dto => dto.DescripcionTipoEquipo, mce => mce.MapFrom(exp => exp.Boleta.Proveedor.Equipos.FirstOrDefault().EquiposCategoria.Descripcion));
                mapDescargadores.ForMember(dto => dto.DescripcionTipoProducto, mce => mce.MapFrom(exp => exp.Boleta.CategoriaProducto.Descripcion));

                var mapPagoDescargadores = map.CreateMap<PagoDescargadores, PagoDescargadoresDTO>();
                mapPagoDescargadores.ForMember(dto => dto.EncargadoCuadrilla, mce => mce.MapFrom(exp => exp.Cuadrilla.NombreEncargado));
                mapPagoDescargadores.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.Cuadrilla.ClientePlanta.NombrePlanta));
                mapPagoDescargadores.ForMember(dto => dto.PlantaId, mce => mce.MapFrom(exp => exp.Cuadrilla.PlantaId));
                mapPagoDescargadores.ForMember(dto => dto.FechaTransaccion, mce => mce.MapFrom(exp => exp.Descargadores.FirstOrDefault().FechaTransaccion));
                mapPagoDescargadores.ForMember(dto => dto.TotalPago, mce => mce.MapFrom(exp => exp.ObtenerTotalPago()));
                mapPagoDescargadores.ForMember(dto => dto.TotalJustificado, mce => mce.MapFrom(exp => exp.ObtenerPagoTotalJustificado()));
                mapPagoDescargadores.ForMember(dto => dto.TotalBoletaDescargas, mce => mce.MapFrom(exp => exp.ObtenerTotalDescargas()));
                mapPagoDescargadores.ForMember(dto => dto.HasShippingNumber, mce => mce.MapFrom(exp => exp.HasShippingNumber()));

                var mapPagoDescargaDetalle = map.CreateMap<PagoDescargaDetalles, PagoDescargaDetallesDTO>();
                mapPagoDescargaDetalle.ForMember(dto => dto.BancoId, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.BancoId));
                mapPagoDescargaDetalle.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.LineaCredito.CodigoLineaCredito + " - " + exp.LineaCredito.CuentasFinanciera.CuentaNo));
                mapPagoDescargaDetalle.ForMember(dto => dto.PuedeEditarCreditoDeduccion, mce => mce.MapFrom(exp => exp.LineaCredito.Estado == "ACTIVO"));
                mapPagoDescargaDetalle.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.Descripcion));

                var mapGasolineraCreditos = map.CreateMap<GasolineraCreditos, GasolineraCreditosDTO>();
                mapGasolineraCreditos.ForMember(dto => dto.NombreGasolinera, mce => mce.MapFrom(exp => exp.Gasolinera.Descripcion));
                mapGasolineraCreditos.ForMember(dto => dto.SaldoAnterior, mce => mce.MapFrom(exp => exp.Saldo));
                mapGasolineraCreditos.ForMember(dto => dto.SaldoActual, mce => mce.MapFrom(exp => exp.ObtenerSaldoActual()));
                mapGasolineraCreditos.ForMember(dto => dto.Debito, mce => mce.MapFrom(exp => exp.ObtenerDebitoTotal()));

                var mapGasolineraCreditoPagos = map.CreateMap<GasolineraCreditoPagos, GasolineraCreditoPagosDTO>();
                mapGasolineraCreditoPagos.ForMember(dto => dto.BancoId, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.BancoId));
                mapGasolineraCreditoPagos.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.LineaCredito.CuentasFinanciera.Banco.Descripcion));
                mapGasolineraCreditoPagos.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.LineaCredito.CodigoLineaCredito + " - " + exp.LineaCredito.CuentasFinanciera.CuentaNo));
                mapGasolineraCreditoPagos.ForMember(dto => dto.PuedeEditarCreditoDeduccion, mce => mce.MapFrom(exp => exp.LineaCredito.Estado == "ACTIVO"));

                var mapFacturas = map.CreateMap<Factura, FacturasDTO>();
                mapFacturas.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.ClientePlanta.NombrePlanta));
                mapFacturas.ForMember(dto => dto.NombreSubPlanta, mce => mce.MapFrom(exp => exp.SubFacility.NombreSubPlanta));
                mapFacturas.ForMember(dto => dto.NombreSucursal, mce => mce.MapFrom(exp => exp.Sucursal.Nombre));
                mapFacturas.ForMember(dto => dto.Tax, mce => mce.MapFrom(exp => exp.GetInvoiceTax()));
                mapFacturas.ForMember(dto => dto.SubTotalDollar, mce => mce.MapFrom(exp => exp.GetInvoiceSubTotalDollar()));
                mapFacturas.ForMember(dto => dto.SubTotalLps, mce => mce.MapFrom(exp => exp.GetInvoiceSubTotalLps()));
                mapFacturas.ForMember(dto => dto.InvoiceForeignTotal, mce => mce.MapFrom(exp => exp.GetInvoiceForeignCurrencyTotal()));
                mapFacturas.ForMember(dto => dto.InvoiceLocalTotal, mce => mce.MapFrom(exp => exp.GetInvoiceLocalCurrencyTotal()));
                mapFacturas.ForMember(dto => dto.ShippingNumberRequired, mce => mce.MapFrom(exp => exp.IsShippingNumberRequired()));
                mapFacturas.ForMember(dto => dto.HasSubPlanta, mce => mce.MapFrom(exp => exp.ClientePlanta.HasSubPlanta));
                mapFacturas.ForMember(dto => dto.RequiereOrdenCompra, mce => mce.MapFrom(exp => exp.ClientePlanta.RequiresPurchaseOrder));
                mapFacturas.ForMember(dto => dto.RequiereProForm, mce => mce.MapFrom(exp => exp.ClientePlanta.RequiresProForm));

                var mapFacturaDetalle = map.CreateMap<FacturaDetalleBoletas, FacturaDetalleBoletasDTO>();
                mapFacturaDetalle.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapFacturaDetalle.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.Boleta.NumeroEnvio));
                mapFacturaDetalle.ForMember(dto => dto.Motorista, mce => mce.MapFrom(exp => exp.Boleta.Motorista));
                mapFacturaDetalle.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.Boleta.Proveedor.Nombre));
                //mapFacturaDetalle.ForMember(dto => dto.PesoProducto, mce => mce.MapFrom(exp => exp.Boleta.PesoProducto));
                mapFacturaDetalle.ForMember(dto => dto.PrecioProductoCompra, mce => mce.MapFrom(exp => exp.Boleta.PrecioProductoCompra));
                mapFacturaDetalle.ForMember(dto => dto.PrecioVenta, mce => mce.MapFrom(exp => exp.GetSalePrice()));
                mapFacturaDetalle.ForMember(dto => dto.PesoProductoCompra, mce => mce.MapFrom(exp => exp.GetPurchaseWeight()));
                mapFacturaDetalle.ForMember(dto => dto.Profit, mce => mce.MapFrom(exp => exp.GetProfit()));
                mapFacturaDetalle.ForMember(dto => dto.HasErrorWeightOrPrice, mce => mce.MapFrom(exp => exp.HasErrorWeightOrPrice()));
                mapFacturaDetalle.ForMember(dto => dto.FechaSalida, mce => mce.MapFrom(exp => exp.Boleta.FechaSalida));
                mapFacturaDetalle.ForMember(dto => dto.PlacaEquipo, mce => mce.MapFrom(exp => exp.Boleta.PlacaEquipo));
                mapFacturaDetalle.ForMember(dto => dto.DescripcionTipoProducto, mce => mce.MapFrom(exp => exp.Boleta.CategoriaProducto.Descripcion));

                var mapFacturaCategorias = map.CreateMap<FacturasCategorias, FacturasCategoriasDTO>();

                var mapRecibos = map.CreateMap<Recibos, RecibosDTO>();

                map.CreateMap<ReporteFacturacion, ReporteFacturacionDTO>();
                map.CreateMap<ReportePagoDescargadores, ReportePagoDescargadoresDTO>();

                map.CreateMap<CuentasFinancieras, CuentasFinancierasDTO>();
                map.CreateMap<CuentasFinancieraTipos, CuentasFinancieraTiposDTO>();

                var mapLineasCredito = map.CreateMap<LineasCredito, LineasCreditoDTO>();
                mapLineasCredito.ForMember(dto => dto.NombreSucursal, mce => mce.MapFrom(exp => exp.Sucursal.Nombre));
                mapLineasCredito.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.CuentasFinanciera.Banco.Descripcion));
                mapLineasCredito.ForMember(dto => dto.CuentaFinanciera, mce => mce.MapFrom(exp => exp.CuentasFinanciera.CuentaNo));
                mapLineasCredito.ForMember(dto => dto.TipoCredito, mce => mce.MapFrom(exp => exp.CuentasFinanciera.CuentasFinancieraTipos.Descripcion));
                mapLineasCredito.ForMember(dto => dto.RequiereBanco, mce => mce.MapFrom(exp => exp.CuentasFinanciera.CuentasFinancieraTipos.RequiereBanco));
                mapLineasCredito.ForMember(dto => dto.DeduccionTotal, mce => mce.MapFrom(exp => exp.ObtenerDeduccionTotal()));
                mapLineasCredito.ForMember(dto => dto.CreditoDisponible, mce => mce.MapFrom(exp => exp.ObtenerCreditoDisponible()));

                map.CreateMap<LineasCreditoDeducciones, LineasCreditoDeduccionesDTO>();
                map.CreateMap<MaestroBiomasa, MaestroBiomasaDTO>();

                var mapOrdenesCompraProducto = map.CreateMap<OrdenesCompraProducto, OrdenesCompraProductoDTO>();
                mapOrdenesCompraProducto.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.ClientePlanta.NombrePlanta));
                mapOrdenesCompraProducto.ForMember(dto => dto.MontoLPS, mce => mce.MapFrom(exp => exp.CalcularTotalLpsPO()));
                mapOrdenesCompraProducto.ForMember(dto => dto.PorcentajeCumplimiento, mce => mce.MapFrom(exp => exp.CalcularRendimientoEntrega()));
                mapOrdenesCompraProducto.ForMember(dto => dto.TotalDetalleDollares, mce => mce.MapFrom(exp => exp.CalcularTotalDollaresDetallePO()));
                mapOrdenesCompraProducto.ForMember(dto => dto.TotalDetalleLps, mce => mce.MapFrom(exp => exp.CalcularTotalLpsDetallePO()));

                var mapOrdenesCompraProductoDetlle = map.CreateMap<OrdenesCompraProductoDetalle, OrdenesCompraProductoDetalleDTO>();
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.OrdenCompraProductoNo, mce => mce.MapFrom(exp => exp.OrdenesCompraProducto.OrdenCompraNo));
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.PrecioLPS, mce => mce.MapFrom(exp => exp.CalcularPrecioLpsPorTonelada()));
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.TotalDollares, mce => mce.MapFrom(exp => exp.ObtenerTotalDollaresPO()));
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.TotalLps, mce => mce.MapFrom(exp => exp.ObtenerTotalLpsPO()));
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.ToneladasAsignadas, mce => mce.MapFrom(exp => exp.ObtenerTotalToneladasProductoEntregado()));
                mapOrdenesCompraProductoDetlle.ForMember(dto => dto.CumplimientoDetalleEntrega, mce => mce.MapFrom(exp => exp.CalcularCumplimientoDetalleEntrega()));

                var mapOrdenesCompraDetalleBoleta = map.CreateMap<OrdenesCompraDetalleBoleta, OrdenesCompraDetalleBoletaDTO>();
                mapOrdenesCompraDetalleBoleta.ForMember(dto => dto.OrdenCompraProductoNo, mce => mce.MapFrom(exp => exp.OrdenesCompraProducto.OrdenCompraNo));
                mapOrdenesCompraDetalleBoleta.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));

                var mapBoletaOtrasDeducciones = map.CreateMap<BoletaOtrasDeducciones, BoletaOtrasDeduccionesDTO>();
                mapBoletaOtrasDeducciones.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapBoletaOtrasDeducciones.ForMember(dto => dto.BancoId, mce => mce.MapFrom(exp => exp.LineasCredito.CuentasFinanciera.Banco.BancoId));
                mapBoletaOtrasDeducciones.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.LineasCredito.CuentasFinanciera.Banco.Descripcion));
                mapBoletaOtrasDeducciones.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.LineasCredito.CodigoLineaCredito));
                mapBoletaOtrasDeducciones.ForMember(dto => dto.PuedeEliminarBoletaOtraDeduccion, mce => mce.MapFrom(exp => exp.LineasCredito.Estado == "ACTIVO"));

                map.CreateMap<ReporteBoletaDeduccionesPrestamo, ReporteBoletaDeduccionesPrestamoDTO>();
                map.CreateMap<ReporteDescargadores, ReporteDescargadoresDTO>();
                map.CreateMap<RptGasCreditoEncabezado, RptGasCreditoEncabezadoDTO>();
                map.CreateMap<RptOrdenesCombustibleOperativo, RptOrdenesCombustibleOperativoDTO>();
                map.CreateMap<RptOrdenesCombustiblePersonales, RptOrdenesCombustiblePersonalesDTO>();

                var mapPrestamosEncabezado = map.CreateMap<RptPrestamosPorProveedorEncabezado, RptPrestamosPorProveedorEncabezadoDTO>();
                mapPrestamosEncabezado.ForMember(dto => dto.SaldoPendiente, mce => mce.MapFrom(exp => exp.TotalCobrar - exp.TotalAbono));
                
                map.CreateMap<RptPrestamosPorProveedorDetalle, RptPrestamosPorProveedorDetalleDTO>();

                var mapHistorialBoletas = map.CreateMap<RptHistorialBoletas, RptHistorialDatoBoletasDTO>();
                var mapPrestamoPendientes = map.CreateMap<RptPrestamosPendientes, RptDatosPrestamosPendientes>();
                mapPrestamoPendientes.ForMember(dto => dto.SaldoPendiente, mce => mce.MapFrom(exp => exp.ObtenerSaldoPendientePorProveedor()));

                var mapDescargasPorAdelanto = map.CreateMap<DescargasPorAdelantado, DescargasPorAdelantadoDTO>();
                mapDescargasPorAdelanto.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.GetCodigoBoleta()));
                mapDescargasPorAdelanto.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.GetNumeroBoleta()));

                map.CreateMap<RptCompraProductoBiomasaResumen, CompraProductoBiomasaResumenDato>();
                map.CreateMap<RptCompraProductoBiomasaDetalle, CompraProductoBiomasaDetalleDato>();

                map.CreateMap<RptBoletaPaymentPendingResumen, RptBoletaPaymentPendingResumenDto>();
                map.CreateMap<RptBoletaPaymentPending, RptBoletaPaymentPendingDto>();

                var mapBoletasHumedad = map.CreateMap<BoletaHumedad, BoletaHumedadDto>();
                mapBoletasHumedad.ForMember(dto => dto.OutStandingPay, mce => mce.MapFrom(exp => exp.CalculateHumidityPricePayment()));
                mapBoletasHumedad.ForMember(dto => dto.NombreProveedor, mce => mce.MapFrom(exp => exp.BoletaHumedadAsignacion.Boleta.Proveedor.Nombre));
                mapBoletasHumedad.ForMember(dto => dto.PrecioCompra, mce => mce.MapFrom(exp => exp.BoletaHumedadAsignacion.Boleta.PrecioProductoCompra));
                mapBoletasHumedad.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.BoletaHumedadAsignacion.Boleta.CodigoBoleta));
                mapBoletasHumedad.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.BoletaHumedadAsignacion.Boleta.ClientePlanta.NombrePlanta));

                var mapBoletasHumedadAsignacion = map.CreateMap<BoletaHumedadAsignacion, BoletaHumedadAsignacionDto>();
                mapBoletasHumedadAsignacion.ForMember(dto => dto.OutStandingPay, mce => mce.MapFrom(exp => exp.BoletaHumedad.CalculateHumidityPricePayment()));
                mapBoletasHumedadAsignacion.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.BoletaHumedad.NumeroEnvio));
                mapBoletasHumedadAsignacion.ForMember(dto => dto.NombrePlanta, mce => mce.MapFrom(exp => exp.BoletaHumedad.ClientePlanta.NombrePlanta));
                mapBoletasHumedadAsignacion.ForMember(dto => dto.Estado, mce => mce.MapFrom(exp => exp.BoletaHumedad.Estado));
                mapBoletasHumedadAsignacion.ForMember(dto => dto.PorcentajeTolerancia, mce => mce.MapFrom(exp => exp.BoletaHumedad.PorcentajeTolerancia));
                mapBoletasHumedadAsignacion.ForMember(dto => dto.HumedadPromedio, mce => mce.MapFrom(exp => exp.BoletaHumedad.HumedadPromedio));

                var mapBoletasHumedadPagos = map.CreateMap<BoletaHumedadPago, BoletaHumedadPagoDto>();
                mapBoletasHumedadPagos.ForMember(dto => dto.TotalHumidityPayment, mce => mce.MapFrom(exp => exp.BoletaHumedad.CalculateHumidityPricePayment()));
                mapBoletasHumedadPagos.ForMember(dto => dto.PorcentajeTolerancia, mce => mce.MapFrom(exp => exp.BoletaHumedad.PorcentajeTolerancia));
                mapBoletasHumedadPagos.ForMember(dto => dto.HumedadPromedio, mce => mce.MapFrom(exp => exp.BoletaHumedad.HumedadPromedio));
                mapBoletasHumedadPagos.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.BoletaHumedad.NumeroEnvio));
                mapBoletasHumedadPagos.ForMember(dto => dto.PrecioProducto, mce => mce.MapFrom(exp => exp.Boleta.PrecioProductoCompra));
                mapBoletasHumedadPagos.ForMember(dto => dto.Motorista, mce => mce.MapFrom(exp => exp.Boleta.Motorista));
                mapBoletasHumedadPagos.ForMember(dto => dto.ProductoBiomasa, mce => mce.MapFrom(exp => exp.Boleta.CategoriaProducto.Descripcion));
                mapBoletasHumedadPagos.ForMember(dto => dto.PlacaCabezal, mce => mce.MapFrom(exp => exp.Boleta.PlacaEquipo));
                mapBoletasHumedadPagos.ForMember(dto => dto.Toneladas, mce => mce.MapFrom(exp => exp.Boleta.PesoProducto));
                mapBoletasHumedadPagos.ForMember(dto => dto.FechaIngreso, mce => mce.MapFrom(exp => exp.Boleta.FechaCreacionBoleta));

                map.CreateMap<RptHumidityPendingPayment, RptHumidityPendingPaymentDto>();

                var mapBoletaDeduccionesManual = map.CreateMap<BoletaDeduccionManual, BoletaDeduccionManualDto>();

                var mapFacturaDetalleItem = map.CreateMap<FacturaDetalleItem, FacturaDetalleItemDto>();
                mapFacturaDetalleItem.ForMember(dto => dto.ProductoDescripcion, mce => mce.MapFrom(exp => exp.CategoriaProducto.Descripcion));
                mapFacturaDetalleItem.ForMember(dto => dto.IsForeignCurrency, mce => mce.MapFrom(exp => exp.Factura.IsForeignCurrency));
                mapFacturaDetalleItem.ForMember(dto => dto.IsExonerated, mce => mce.MapFrom(exp => exp.Factura.IsExonerated));
                mapFacturaDetalleItem.ForMember(dto => dto.TaxPercent, mce => mce.MapFrom(exp => exp.Factura.TaxPercent));
                mapFacturaDetalleItem.ForMember(dto => dto.TotalInvoice, mce => mce.MapFrom(exp => exp.Factura.Total));
                mapFacturaDetalleItem.ForMember(dto => dto.ForeignCurrencyItemAmount, mce => mce.MapFrom(exp => exp.GetForeignCurrencyItemAmount()));
                mapFacturaDetalleItem.ForMember(dto => dto.LocalCurrencyItemAmount, mce => mce.MapFrom(exp => exp.GetLocalCurrencyItemAmount()));
                mapFacturaDetalleItem.ForMember(dto => dto.TaxItem, mce => mce.MapFrom(exp => exp.GetTaxItemLps()));

                var mapSubPlantas = map.CreateMap<SubPlanta, SubPlantaDto>();
                var mapFacturaPagos = map.CreateMap<FacturaPago, FacturaPagoDto>();
                mapFacturaPagos.ForMember(dto => dto.NombreBanco, mce => mce.MapFrom(exp => exp.Bank.Descripcion));
                var mapNotasCredito = map.CreateMap<NotaCredito, NotaCreditoDto>();

                var mapBonificaciones = map.CreateMap<BonificacionProducto, BonificacionProductoDto>();

                map.CreateMap<RptPendingInvoice, RptPendingInvoiceDto>();
                map.CreateMap<RptBoletaWithOutInvoice, RptBoletaWithOutInvoiceDto>();
                map.CreateMap<RptBillWithWeightsError, RptBillWithWeightsErrorDto>();

                map.CreateMap<AjusteTipo, AjusteTipoDto>();

                var mapAjusteBoletas = map.CreateMap<AjusteBoleta, AjusteBoletaDto>();
                mapAjusteBoletas.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.Boleta.CodigoBoleta));
                mapAjusteBoletas.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => string.IsNullOrEmpty(exp.Boleta.NumeroEnvio) ? "N/A" : exp.Boleta.NumeroEnvio));
                mapAjusteBoletas.ForMember(dto => dto.Proveedor, mce => mce.MapFrom(exp => exp.Boleta.Proveedor.Nombre));
                mapAjusteBoletas.ForMember(dto => dto.TipoProducto, mce => mce.MapFrom(exp => exp.Boleta.CategoriaProducto.Descripcion));
                mapAjusteBoletas.ForMember(dto => dto.FechaSalida, mce => mce.MapFrom(exp => exp.Boleta.FechaSalida));
                mapAjusteBoletas.ForMember(dto => dto.Planta, mce => mce.MapFrom(exp => exp.Boleta.ClientePlanta.NombrePlanta));
                mapAjusteBoletas.ForMember(dto => dto.Total, mce => mce.MapFrom(exp => exp.GetAjusteBoletaTotal()));
                mapAjusteBoletas.ForMember(dto => dto.Abonos, mce => mce.MapFrom(exp => exp.GetAjusteBoletaPayments()));
                mapAjusteBoletas.ForMember(dto => dto.Saldo, mce => mce.MapFrom(exp => exp.GetAjusteBoletaPendingAmount()));

                var mapAjusteBoletaDetalles = map.CreateMap<AjusteBoletaDetalle, AjusteBoletaDetalleDto>();
                mapAjusteBoletaDetalles.ForMember(dto => dto.AjusteTipoDescripcion, mce => mce.MapFrom(exp => exp.AjusteTipo.Descripcion));
                mapAjusteBoletaDetalles.ForMember(dto => dto.NoDocumento, mce => mce.MapFrom(exp => exp.GetDocumentRef()));
                mapAjusteBoletaDetalles.ForMember(dto => dto.CodigoLineaCredito, mce => mce.MapFrom(exp => exp.GetCreditLine()));
                mapAjusteBoletaDetalles.ForMember(dto => dto.Estado, mce => mce.MapFrom(exp => exp.GetStatus()));
                mapAjusteBoletaDetalles.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.AjusteBoleta.Boleta.CodigoBoleta));
                mapAjusteBoletaDetalles.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.AjusteBoleta.GetNumeroEnvio()));
                mapAjusteBoletaDetalles.ForMember(dto => dto.SaldoPendiente, mce => mce.MapFrom(exp => exp.GetSaldoPendiente()));
                mapAjusteBoletaDetalles.ForMember(dto => dto.CadenaAjuste, mce => mce.MapFrom(exp => exp.GetCadenaAjuste()));

                var mapAjusteBoletaPagos = map.CreateMap<AjusteBoletaPago, AjusteBoletaPagoDto>();
                mapAjusteBoletaPagos.ForMember(dto => dto.CodigoBoleta, mce => mce.MapFrom(exp => exp.GetCodigoBoletaPayment()));
                mapAjusteBoletaPagos.ForMember(dto => dto.NumeroEnvio, mce => mce.MapFrom(exp => exp.GetNumeroEnvioPayment()));
                mapAjusteBoletaPagos.ForMember(dto => dto.Planta, mce => mce.MapFrom(exp => exp.Boleta.ClientePlanta.NombrePlanta));

                map.CreateMap<BoletaImg, BoletaImgDto>();
                map.CreateMap<OrdenCombustibleImg, OrdenCombustibleImgDto>();
                map.CreateMap<RptHistoryOfInvoiceBalances, RptHistoryOfInvoiceBalancesDto>();

                var mapFuelOrderManualPayments = map.CreateMap<FuelOrderManualPayment, FuelOrderManualPaymentDto>();
                mapFuelOrderManualPayments.ForMember(dto => dto.BankName, mce => mce.MapFrom(exp => exp.GetBankName()));
            });
        }
    }
}