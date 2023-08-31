using ComercialColindres.DTOs.RequestDTOs.Boletas;
using ComercialColindres.DTOs.RequestDTOs.Conductores;
using ComercialColindres.DTOs.RequestDTOs.CuentasBancarias;
using ComercialColindres.DTOs.RequestDTOs.Descargadores;
using ComercialColindres.DTOs.RequestDTOs.Equipos;
using ComercialColindres.DTOs.RequestDTOs.Prestamos;
using ComercialColindres.DTOs.RequestDTOs.PrestamosTransferencias;
using ComercialColindres.DTOs.RequestDTOs.Proveedores;
using ComercialColindres.DTOs.RequestDTOs.Usuarios;
using ComercialColindres.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComercialColindres.DTOs.RequestDTOs.Cuadrillas;
using ComercialColindres.DTOs.RequestDTOs.PagoDescargadores;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCombustible;
using ComercialColindres.DTOs.RequestDTOs.GasolineraCreditos;
using ComercialColindres.DTOs.RequestDTOs.Facturas;
using ComercialColindres.DTOs.RequestDTOs.LineasCredito;
using ComercialColindres.DTOs.RequestDTOs.OrdenesCompraProducto;
using ComercialColindres.DTOs.RequestDTOs;
using ComercialColindres.DTOs.RequestDTOs.FacturaDetalleItems;
using ComercialColindres.DTOs.RequestDTOs.AjusteBoletas;

namespace ComercialColindres.DatosEjemplos
{
    public class DatosDiseño
    {
        #region usuarios
        internal static List<UsuariosDTO> ListaUsuarios()
        {
            var listado = new List<UsuariosDTO>
            {
                CrearUsuarioDTO("ecruzj", "Josue Cruz", Estados.ACTIVO),
                CrearUsuarioDTO("mesther", "Marie Esther Cruz", Estados.ACTIVO),
                CrearUsuarioDTO("cmenjivar", "Cindy Menjivar", Estados.INACTIVO)
            };

            return listado;
        }

        static UsuariosDTO CrearUsuarioDTO(string usuario, string nombre, string estado)
        {
            return new UsuariosDTO
            {
                Usuario = usuario,
                Nombre = nombre,
                Estado = estado,
                UsuariosOpciones = new List<UsuariosOpcionesDTO>
                {
                    new UsuariosOpcionesDTO
                    {
                        NombreOpcion = "Ventas",
                        NombreSucursal = "Sucursal#1"
                    },
                    new UsuariosOpcionesDTO
                    {
                        NombreOpcion = "Productos",
                        NombreSucursal = "Sucursal#1"
                    }
                },
                UsuariosSucursalesAsignadas = new List<UsuariosSucursalesAsignadasDTO>
                {
                    new UsuariosSucursalesAsignadasDTO
                    {
                        Nombre = "Sucursal#1",
                        SucursalId = 1
                    }
                }
            };
        }
        #endregion

        #region proveedores
        internal static List<ProveedoresDTO> ListaProveedores()
        {
            var listado = new List<ProveedoresDTO>
            {
                CrearProveedorDTO("0504198200201", "0501-1992-00882", "Josue Cruz", "SAN PEDRO SULA, Col San Juan", "2323-2322", "ecruzj@outlook.com", Estados.ACTIVO),
                CrearProveedorDTO("0504168200318", "0501-1989-08237", "Cind Menjivar", "SAN PEDRO SULA, Residencial Potosi", "2525-7864", "cmenjivar@outlook.com", Estados.INACTIVO),
                CrearProveedorDTO("0834238943509", "0501-2014-00340", "Marie Esther Cruz", "SAN PEDRO SULA, Residencial San Roberton", "2553-1896", "mesther.cruz@outlook.com", Estados.ACTIVO),
            };
            return listado;
        }

        static ProveedoresDTO CrearProveedorDTO(string rtn, string cedulaNo, string nombre, string direccion, string telefonos,
                                           string correoElectronico, string estado)
        {
            return new ProveedoresDTO
            {
                RTN = rtn,
                CedulaNo = cedulaNo,
                Nombre = nombre,
                Direccion = direccion,
                Telefonos = telefonos,
                CorreoElectronico = correoElectronico,
                Estado = estado,
            };
        }

        internal static List<CuentasBancariasDTO> ListaCuentasBancarias()
        {
            var listado = new List<CuentasBancariasDTO>
            {
                CrearCuentaBancariaDTO(1, 1, "0501-1992-00882", "22-98343-98", "Josue Cruz", "Ficohsa"),
                CrearCuentaBancariaDTO(1, 1, "0501-1992-00882", "8945-83453-1230", "Josue Cruz", "Banco Occidente"),
                CrearCuentaBancariaDTO(1, 1, "0501-2014-89453", "0984-12355-874", "Marie Esther Cruz", "BanPais"),
            };

            return listado;
        }

        static CuentasBancariasDTO CrearCuentaBancariaDTO(int proveedorId, int bancoId, string cedula, string cuentaNo, string abonado, string nombreBanco)
        {
            return new CuentasBancariasDTO
            {
                ProveedorId = proveedorId,
                BancoId = bancoId,
                CedulaNo = cedula,
                CuentaNo = cuentaNo,
                NombreAbonado = abonado,
                NombreBanco = nombreBanco,
                Estado = "ACTIVO"
            };
        }

        internal static List<EquiposDTO> ListaEquipos()
        {
            var listado = new List<EquiposDTO>
            {
                CrearEquiposDTO(1, 1, "MPG4563", "CAMION"),
                CrearEquiposDTO(1, 1, "PHR8564", "RATRA"),
                CrearEquiposDTO(1, 1, "FTP2094", "VOLEQUETA"),
            };

            return listado;
        }

        static EquiposDTO CrearEquiposDTO(int equipoCategoria, int proveedorId, string placaCabezal, string categoria)
        {
            return new EquiposDTO
            {
                EquipoCategoriaId = equipoCategoria,
                ProveedorId = proveedorId,
                Estado = "ACTIVO",
                DescripcionCategoria = categoria,
                PlacaCabezal = placaCabezal
            };
        }

        internal static List<ConductoresDTO> ListaConductores()
        {
            var listado = new List<ConductoresDTO>
            {
                CrearConductores(1, "Pedro Villalobo", "9843-8644"),
                CrearConductores(1, "Juan Villatoro", "3565-8450"),
                CrearConductores(1, "Fernando Castro", "9763-1232"),
            };

            return listado;
        }

        static ConductoresDTO CrearConductores(int proveedorId, string nombreConductor, string telefonoConductor)
        {
            return new ConductoresDTO
            {
                ProveedorId = proveedorId,
                Nombre = nombreConductor,
                Telefonos = telefonoConductor
            };
        }
        #endregion

        #region Boletas

        internal static List<BoletasDTO> ListaBoletas()
        {
            var listado = new List<BoletasDTO>
            {
                CrearBoletaDTO("00087634", "00763433", "PHT 8976", "Juan Perez", "Acerrin Nuevo", "Francisco Zepeda", "Caracol Knits", "Lucho", 650.00m, 28.70m, 850.00m, "Rastra", DateTime.Now, "ACTIVO"),
                CrearBoletaDTO("00023434", "008767723", "FTP 8736", "Ismael Ramiréz", "Acerrin Viejo", "Marlon Díaz", "Merendon", "Lucho", 450.00m, 12.20m, 650.00m, "Camion", DateTime.Now, "CERRADO"),
                CrearBoletaDTO("007623423", "00682348342", "PTH 9923", "Jorge Lopez", "Acerrin Viejo", "Osiris Paz", "Gildan", "Colocho", 600.00m, 27.60m, 720.00m, "Rastra", DateTime.Now, "ANULADO"),
            };
            return listado;
        }

        static BoletasDTO CrearBoletaDTO(string codigoBoleta, string numeroEnvio, string placaCabezal, string motorista, string descripcionTipoProducto,
                                         string nombreProveedor, string nombrePlanta, string nombreDescargador, decimal precioDescarga, decimal pesoProducto,
                                         decimal precioProducto, string descripcionTipoEquipo, DateTime fechaSalida, string estado)
        {
            return new BoletasDTO
            {
                CodigoBoleta = codigoBoleta,
                NumeroEnvio = numeroEnvio,
                PlacaEquipo = placaCabezal,
                Motorista = motorista,
                DescripcionTipoProducto = descripcionTipoProducto,
                NombreProveedor = nombreProveedor,
                NombrePlanta = nombrePlanta,
                NombreDescargador = nombreDescargador,
                PrecioDescarga = precioDescarga,
                PesoProducto = pesoProducto,
                PrecioProductoCompra = precioProducto,
                DescripcionTipoEquipo = descripcionTipoEquipo,
                FechaSalida = fechaSalida,
                Estado = estado
            };
        }

        #endregion

        #region Prestamos

        internal static List<PrestamosDTO> ListaPrestamos()
        {
            var listado = new List<PrestamosDTO>
            {
                CrearPrestamoDTO(1, 1, "PR001", 1, DateTime.Now, "Yamileth Colindres", 8m, 25000m, "ACTIVO", "TEST", "Josue Cruz", 15000m),
                CrearPrestamoDTO(2, 1, "PR002", 1, DateTime.Now, "Yamileth Colindres", 12m, 50000, "CERRADO", "TEST", "Josue Cruz", 36000m),
                CrearPrestamoDTO(3, 1, "PR003", 1, DateTime.Now, "Yamileth Colindres", 10m, 10000, "ANULADO", "TEST", "Josue Cruz", 0m),
            };
            return listado;
        }

        static PrestamosDTO CrearPrestamoDTO(int prestamoId, int sucursalId, string codigoPrestamo, int proveedorId, DateTime fechaPrestamo,
                                             string autorizadoPor, decimal porcentajeInteres, decimal montoPrestamo, string estado, string observaciones,
                                             string nombreProveedor, decimal totalAbono)
        {
            return new PrestamosDTO
            {
                PrestamoId = prestamoId,
                SucursalId = sucursalId,
                CodigoPrestamo = codigoPrestamo,
                ProveedorId = proveedorId,
                FechaCreacion = fechaPrestamo,
                AutorizadoPor = autorizadoPor,
                PorcentajeInteres = porcentajeInteres,
                MontoPrestamo = montoPrestamo,
                Estado = estado,
                Observaciones = observaciones,
                NombreProveedor = nombreProveedor,
                TotalACobrar = montoPrestamo * ((porcentajeInteres / 100) + 1),
                TotalAbono = totalAbono,
                SaldoPendiente = (montoPrestamo * ((porcentajeInteres / 100) + 1)) - totalAbono,
                NombreSucursal = "Comercial Colindres"
            };
        }

        internal static List<PrestamosTransferenciasDTO> ListaPrestamosTransferencias()
        {
            var listado = new List<PrestamosTransferenciasDTO>
            {
                CrearPrestamoTransferencias(1, "Efectivo", null, string.Empty, 2500m),
                CrearPrestamoTransferencias(1, "Interbanca", "Ficohsa", "908234", 5000m),
            };

            return listado;
        }

        static PrestamosTransferenciasDTO CrearPrestamoTransferencias(int prestamosId, string formaDePago, string nombreBanco, string noDocumento, decimal monto)
        {
            return new PrestamosTransferenciasDTO
            {
                PrestamoId = prestamosId,
                FormaDePago = formaDePago,
                NombreBanco = nombreBanco,
                NoDocumento = noDocumento,
                Monto = monto
            };
        }

        #endregion

        #region Descargas

        internal static List<DescargadoresDTO> ListaDescargas()
        {
            var listado = new List<DescargadoresDTO>
            {
                CrearDescarga(1, 1, 2400m, DateTime.Now, "4567945", "ACTIVO"),
                CrearDescarga(2, 1, 1800m, DateTime.Now, "56765334", "CERRADO"),
                CrearDescarga(3, 1, 600m, DateTime.Now, "457654678", "ACTIVO"),
                CrearDescarga(4, 1, 800m, DateTime.Now, "34787566456", "CERRADO")
            };

            return listado;
        }

        internal static List<DescargadoresDTO> ListaDescargasPorPago()
        {
            var listado = new List<DescargadoresDTO>
            {
                CrearDescarga(1, 1, 2400m, DateTime.Now, "4567945", "CERRADO"),
                CrearDescarga(2, 1, 1800m, DateTime.Now, "56765334", "CERRADO"),
                CrearDescarga(3, 1, 600m, DateTime.Now, "457654678", "CERRADO"),
                CrearDescarga(4, 1, 800m, DateTime.Now, "34787566456", "CERRADO")
            };

            return listado;
        }

        static DescargadoresDTO CrearDescarga(int boletaId, int cuadrillaId, decimal precioDescarga, DateTime fechaDescarga, string codigoBoleta, string estado)
        {
            return new DescargadoresDTO
            {
                BoletaId = 1,
                CuadrillaId = 1,
                Estado = estado,
                EstadoBoleta = "ACTIVO",
                FechaDescarga = fechaDescarga,
                PrecioDescarga = precioDescarga,
                EncargadoCuadrilla = "Colocho",
                CodigoBoleta = codigoBoleta
            };
        }

        #endregion

        #region Cuadrillas

        internal static List<CuadrillasDTO> ListaCuadrillas()
        {
            var listado = new List<CuadrillasDTO>
            {
                CrearCuadrilla("Lucho", "Gildan", "ACTIVO"),
                CrearCuadrilla("Gavilán", "Merendon", "ACTIVO"),
                CrearCuadrilla("Cuadrilla Caracol", "Caracol Knits", "ACTIVO")
            };

            return listado;
        }

        static CuadrillasDTO CrearCuadrilla(string nombreEncargado, string nombrePlanta, string estado)
        {
            return new CuadrillasDTO
            {
                NombreEncargado = nombreEncargado,
                NombrePlanta = nombrePlanta,
                Estado = estado
            };
        }

        #endregion

        #region PagoDescargas

        internal static List<PagoDescargadoresDTO> ListaPagoDescargas()
        {
            var listado = new List<PagoDescargadoresDTO>
            {
                CrearPagoDescargadores("2016-00234", "Paul Moreno", "Gildan", 12500m, "ACTIVO"),
                CrearPagoDescargadores("2016-00235", "Colocho", "Gildan", 10500m, "ANULADO"),
                CrearPagoDescargadores("2016-00236", "Ramon Paz", "Merendon", 9600m, "CERRADO"),
            };

            return listado;
        }

        static PagoDescargadoresDTO CrearPagoDescargadores(string codigoDescarga, string encargadoCuadrilla, string nombrePlanta, decimal totalPago, string estado)
        {
            return new PagoDescargadoresDTO
            {
                CodigoPagoDescarga = codigoDescarga,
                EncargadoCuadrilla = encargadoCuadrilla,
                NombrePlanta = nombrePlanta,
                TotalPago = totalPago,
                Estado = estado,
                FechaPago = DateTime.Now
            };
        }

        #endregion

        #region OrdenesCombustible

        internal static List<OrdenesCombustibleDTO> ListaOrdenesCombustible()
        {
            var listado = new List<OrdenesCombustibleDTO>
            {
                CrearOrdenCombustible("Yamileth Colindres", "8345432", "GC16-00983", "Puma Terminal", "PDT-9834", 9300, "67234", "Orden para viaje a Caracol Knits", "Escalante", "ACTIVO"),
                CrearOrdenCombustible("Wilmer Colindres", "9723489", "GC16-00984", "Uno El Eden", "TPP-8234", 8500, "798343", "Orden para viaje a Gildan", "Ricardo Valle", "CERRADO"),
            };

            return listado;
        }

        static OrdenesCombustibleDTO CrearOrdenCombustible(string autorizadoPor, string codigoFactura, string codigoCreditoCombustible,
               string nombreGasolinera, string placaEquipo, decimal monto, string boleta, string observaciones, string nombreProveedor, string estado)
        {
            return new OrdenesCombustibleDTO
            {
                AutorizadoPor = autorizadoPor,
                CodigoFactura = codigoFactura,
                CodigoCreditoCombustible = codigoCreditoCombustible,
                NombreGasolinera = nombreGasolinera,
                PlacaEquipo = placaEquipo,
                Monto = monto,
                FechaCreacion = DateTime.Now,
                CodigoBoleta = boleta,
                Observaciones = observaciones,
                NombreProveedor = nombreProveedor,
                Estado = estado
            };
        }

        #endregion

        #region GasolineraCreditos

        internal static List<GasolineraCreditosDTO> ListaGasolineraCreditos()
        {
            var listado = new List<GasolineraCreditosDTO>
            {
                CrearGasolineraCredito("GC16-00783", "Yamileth Colindres", 380000, DateTime.Now, null, 300000, "Puma Terminal", true, "ACTIVO"),
                CrearGasolineraCredito("GC16-00783", "Yamileth Colindres", 380000, DateTime.Now, null, 300000, "Puma Terminal", true, "ENPROCESO"),
                CrearGasolineraCredito("GC16-00783", "Yamileth Colindres", 380000, DateTime.Now, null, 300000, "Puma Terminal", true, "CERRADO"),
                CrearGasolineraCredito("GC16-00783", "Yamileth Colindres", 380000, DateTime.Now, null, 300000, "Puma Terminal", true, "COMPLETADO"),
                CrearGasolineraCredito("GC16-00783", "Yamileth Colindres", 380000, DateTime.Now, null, 300000, "Puma Terminal", true, "PENDIENTE"),
            };

            return listado;
        }

        static GasolineraCreditosDTO CrearGasolineraCredito(string codigoGasCredito, string creadoPor, decimal credito, DateTime fechaI, DateTime? fechaF,
                                                            decimal saldo, string nombreGasolinera, bool esActual, string estado)
        {
            return new GasolineraCreditosDTO
            {
                CodigoGasCredito = codigoGasCredito,
                CreadoPor = creadoPor,
                Credito = credito,
                FechaInicio = fechaI,
                FechaFinal = fechaF,
                SaldoActual = saldo,
                NombreGasolinera = nombreGasolinera,
                EsCreditoActual = esActual,
                Estado = estado
            };
        }

        #endregion

        #region Facturas

        internal static List<FacturasDTO> ListaFacturas()
        {
            var listado = new List<FacturasDTO>
            {
                CrearFactura("87123", "Caracol Knits", "Comercial Colindres", "Contado", 870000, 0, DateTime.Now, "CERRADO", "Semana 34", false, 0, "99000834"),
                CrearFactura("086234", "Gildan", "Comercial Colindres", "Contado", 950000, 0, DateTime.Now, "CERRADO", "Semana 35", true, 24.45875m, "00987734"),
            };

            return listado;
        }

        static FacturasDTO CrearFactura(string numeroFactura, string nombrePlanta, string sucursal, string tipoFactura, decimal subTotal, decimal saldo, 
                                        DateTime fecha, string estado, string observaciones, bool isForeignCurrency, decimal localCurrencyAmount, string exoneracion)
        {
            return new FacturasDTO
            {
                NumeroFactura = numeroFactura,
                NombrePlanta = nombrePlanta,
                NombreSucursal = sucursal,
                Saldo = saldo,
                Estado = estado,
                Fecha = fecha,
                TipoFactura = tipoFactura,
                Observaciones = observaciones,
                IsForeignCurrency = isForeignCurrency,
                LocalCurrencyAmount = localCurrencyAmount,
                ExonerationNo = exoneracion,
                SubTotalDollar = subTotal,
                TaxPercent = 15,
                Tax = Math.Round(subTotal * 0.15m , 2),
                InvoiceForeignTotal = Math.Round(subTotal + subTotal * 0.15m, 2)
            };
        }

        internal static List<FacturaDetalleItemDto> ListaFacturaDetalleItem()
        {
            var listado = new List<FacturaDetalleItemDto>
            {
                CreateDetalleItem(857.40m, "Aserrin Nuevo", 1050, 2500, 150000),
                CreateDetalleItem(857.40m, "Taco con Aserrin", 1050, 2500, 150000),
                CreateDetalleItem(857.40m, "Raquis Triturado", 1050, 2500, 150000),
                CreateDetalleItem(857.40m, "Fibra de Mesocarpio", 1050, 2500, 150000),
            };

            return listado;
        }

        static FacturaDetalleItemDto CreateDetalleItem(decimal cantidad, string productoDescripcion, decimal precio, decimal totalDollar, decimal totalLps)
        {
            return new FacturaDetalleItemDto
            {
                Cantidad = cantidad,
                ProductoDescripcion = productoDescripcion,
                Precio = precio,
                ForeignCurrencyItemAmount = totalDollar,
                LocalCurrencyItemAmount = totalLps
            };
        }

        #endregion

        #region LineaCreditos
        internal static List<LineasCreditoDTO> ListaLineasCredito()
        {
            var listado = new List<LineasCreditoDTO>
            {
                CrearLineaCredito("LC16-00076", "Comercial Colindres", "Banco Occidente", "23-877892-9873", "Interbanca", "ACTIVO", "88687893", DateTime.Now, null, 1800000, 0 , true)
            };

            return listado;
        }

        static LineasCreditoDTO CrearLineaCredito(string codigoLineaCredito, string nombreSucursal, string nombreBanco, string cuentaFinanciera, string tipoLineaCredito,
                                                  string estado, string noDocumento, DateTime fechaCreacion, DateTime? fechaCierre, decimal montoInicial,
                                                  decimal saldo, bool esLineaCreditoActual)
        {
            return new LineasCreditoDTO
            {
                CodigoLineaCredito = codigoLineaCredito,
                NombreSucursal = nombreSucursal,
                NombreBanco = nombreBanco,
                CuentaFinanciera = cuentaFinanciera,
                TipoCredito = tipoLineaCredito,
                Estado = estado,
                NoDocumento = noDocumento,
                FechaCreacion = fechaCreacion,
                FechaCierre = fechaCierre,
                MontoInicial = montoInicial,
                Saldo = saldo,
                EsLineaCreditoActual = esLineaCreditoActual
            };
        }
        #endregion

        #region Ordenes Compra Producto

        internal static List<OrdenesCompraProductoDTO> ListaOrdenesCompraProducto()
        {
            var listado = new List<OrdenesCompraProductoDTO>
            {
                CrearOrdenCompraProducto("898766782", "675638761", "Gildan", DateTime.Now.AddDays(-2), DateTime.Now, true, "Josue Cruz", 5000.00m, 23.0845m, null, 60.79m, "ACTIVO"),
                CrearOrdenCompraProducto("876662543", "776511230", "Gildan", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-3), false, "Josue Cruz", 5000.00m, 23.0584m, DateTime.Now, 94.89m, "CERRADO")
            };

            return listado;
        }

        static OrdenesCompraProductoDTO CrearOrdenCompraProducto(string ordenCompraNo, string noExoneracionDei, string nombrePlanta, DateTime fechaCreacion, 
                                                                 DateTime fechaActivacion, bool esOrdenActual, string creadoPor, decimal montoDollar, 
                                                                 decimal conversionDollarToLps, DateTime? fechaCierre, decimal porcentajeCumplimiento, string estado)
        {
            return new OrdenesCompraProductoDTO
            {
                OrdenCompraNo = ordenCompraNo,
                NoExoneracionDEI = noExoneracionDei,
                NombrePlanta = nombrePlanta,
                FechaCreacion = fechaCreacion,
                CreadoPor = creadoPor,
                MontoDollar = montoDollar,
                ConversionDollarToLps = conversionDollarToLps,
                FechaActivacion = fechaActivacion,
                EsOrdenCompraActual = esOrdenActual,
                FechaCierre = fechaCierre,
                PorcentajeCumplimiento = porcentajeCumplimiento,
                Estado = estado,
                MontoLPS = montoDollar * conversionDollarToLps
            };
        }

        #endregion

        #region BoletasHumedad

        internal static List<BoletaHumedadDto> ListadoBoletasHumedad()
        {
            List<BoletaHumedadDto> listado = new List<BoletaHumedadDto>
            {
                CreateBoletaHumidity(false, "456543", 56, 54, "32454", "Gildan", "Josue Cruz", "ACTIVO", 960),
                CreateBoletaHumidity(false, "456542", 53, 54, "32456", "Gildan", "Josue Cruz", "CERRADO", 850)
            };

            return listado;
        }

        static BoletaHumedadDto CreateBoletaHumidity(bool boletaIngresada, string codigoBoleta, decimal humedadPromedio, decimal porcentajeTolerancia, string numeroEnvio, string planta, string proveedor, string estado, decimal precioCompra)
        {
            return new BoletaHumedadDto
            {
                BoletaIngresada = boletaIngresada,
                CodigoBoleta = codigoBoleta,
                HumedadPromedio = humedadPromedio,
                PorcentajeTolerancia = porcentajeTolerancia,
                NumeroEnvio = numeroEnvio,
                NombrePlanta = planta,
                NombreProveedor = proveedor,
                Estado = estado,
                PrecioCompra = precioCompra,
                OutStandingPay = humedadPromedio > porcentajeTolerancia ? (humedadPromedio - porcentajeTolerancia) * precioCompra : 0.00m
            };
        }

        #endregion

        internal static List<AjusteBoletaDto> ListadoAjusteBoletas()
        {
            List<AjusteBoletaDto> listado = new List<AjusteBoletaDto>
            {
                CreateAjusteBoleta("987634", string.Empty, "Josue Cruz", "Aserrin Nuevo", DateTime.Now, "HGPC", "CERRADO"),
                CreateAjusteBoleta("987635", "35434", "Wilmer Colindres", "Taco con Aserrin", DateTime.Now, "Gildan", "ACTIVO"),
                CreateAjusteBoleta("987635", string.Empty, "Rigoberto Duarte", "Aserrin Nuevo", DateTime.Now, "HGPC", "ENPROCESO"),
            };

            return listado;
        }

        static AjusteBoletaDto CreateAjusteBoleta(string codigoBoleta, string numeroEnvio, string proveedor, string tipoProducto, DateTime fechaSalida, string planta, string estado)
        {
            return new AjusteBoletaDto
            {
                CodigoBoleta = codigoBoleta,
                NumeroEnvio = numeroEnvio,
                Proveedor = proveedor,
                TipoProducto = tipoProducto,
                FechaSalida = fechaSalida,
                Planta = planta,
                Estado = estado
            };
        }
    }
}
