using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre;
using ComercialColindres.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Test
{
    [TestClass]
    public class BoletasTest
    {
        readonly string boletPath = "";
        private IBoletasDomainServices _boletasDomainServices;
        private LineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;
        private IPagoPrestamosDomainServices _pagoPrestamosDomainServices;
        IDescargadoresDomainServices _descargadoresDomainServices;
        IOrdenCombustibleDomainServices _ordenCombustibleDomainServices;

        readonly Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        IBoletasDetalleDomainServices _boletasDetalleDomainServices;
        IBoletaCierreDomainServices _boletaCierreDomainServices;
        IAjusteBoletaDomainServices _ajusteBoletaDomainServices;
        IBoletaHumedadDomainServices _boletaHumedadDomainServices;
        IBoletaHumedadPagoDomainServices _boletaHumedadPagoDomainServices;
        IBoletaHumedadAsignacionDomainServices _boletaHumedadAsignacionDomainServices;
        ClientePlantas gildan;
        ClientePlantas hgpc;
        CategoriaProductos raquis;
        CategoriaProductos mesocarpio;
        CategoriaProductos taco;
        CategoriaProductos cascaraCafe;
        CategoriaProductos casullaCafe;
        CategoriaProductos aserrin;
        CategoriaProductos maderaEnRollo;
        CategoriaProductos piller;
        byte[] _tempImage;
        private LineasCredito _creditLine;
        List<Deducciones> _deductions;

        [TestInitialize]
        public void Inicializacion()
        {
            _lineasCreditoDeduccionesDomainServices = new LineasCreditoDeduccionesDomainServices();
            _pagoPrestamosDomainServices = new PagoPrestamosDomainServices();
            _descargadoresDomainServices = new DescargadoresDomainServices(_ajusteBoletaDomainServices);
            _ordenCombustibleDomainServices = new OrdenCombustibleDomainServices();
            _boletasDetalleDomainServices = new BoletasDetalleDomainServices();
            _boletaCierreDomainServices = new BoletaCierreDomainServices(_lineasCreditoDeduccionesDomainServices);
            _ajusteBoletaDomainServices = new AjusteBoletaDomainServices(_lineasCreditoDeduccionesDomainServices);
            _boletaHumedadDomainServices = new BoletaHumedadDomainServices(_boletaHumedadPagoDomainServices);
            _boletaHumedadAsignacionDomainServices = new BoletaHumedadAsignacionDomainServices();
            _boletaHumedadPagoDomainServices = new BoletaHumedadPagoDomainServices(_boletaHumedadAsignacionDomainServices);
            

            _boletasDomainServices = new BoletasDomainServices(_lineasCreditoDeduccionesDomainServices, _pagoPrestamosDomainServices, _descargadoresDomainServices, 
                                                               _ordenCombustibleDomainServices, _boletasDetalleDomainServices, _boletaCierreDomainServices,
                                                               _ajusteBoletaDomainServices, _boletaHumedadDomainServices);
            _creditLine = new LineasCredito("LC21-00258", 1, 1, "FT202185485BTY", "Transferencia de Ficohsa", DateTime.Now, 150000, 7800, "ecruzj")
            {
                LineaCreditoId = 78
            };

            _tempImage = new byte[7];
            vendor.ProveedorId = 1;
            gildan = new ClientePlantas("RTN", "Gildan", "5554", "Aldea Rio Nance")
            {
                PlantaId = 2
            };
            hgpc = new ClientePlantas("RTN", "HGPC", "5554", "Choloma, Cortés")
            {
                PlantaId = 5
            };

            raquis = new CategoriaProductos(1, "Raquis")
            {
                CategoriaProductoId = 1
            };

            mesocarpio = new CategoriaProductos(1, "Mesocarpio")
            {
                CategoriaProductoId = 2
            };

            taco = new CategoriaProductos(1, "Taco con Aserrin")
            {
                CategoriaProductoId = 3
            };

            cascaraCafe = new CategoriaProductos(1, "Cascara de Café")
            {
                CategoriaProductoId = 4
            };

            casullaCafe = new CategoriaProductos(1, "Casulla de Café")
            {
                CategoriaProductoId = 5
            };

            CategoriaProductos categoriaProductos = new CategoriaProductos(1, "Aserrin Nuevo")
            {
                CategoriaProductoId = 6
            };
            aserrin = categoriaProductos;

            maderaEnRollo = new CategoriaProductos(1, "Madera en Rollo")
            {
                CategoriaProductoId = 7
            };

            piller = new CategoriaProductos(1, "Piller")
            {
                CategoriaProductoId = 8
            };

            _deductions = new List<Deducciones>
            {
                new Deducciones{DeduccionId = 1, Descripcion = "Prestamo Efectivo"},
                new Deducciones{DeduccionId = 2, Descripcion = "Orden Combustible"},
                new Deducciones{DeduccionId = 3, Descripcion = "Descarga de Producto"},
                new Deducciones{DeduccionId = 4, Descripcion = "Tasa Seguridad"},
                new Deducciones{DeduccionId = 5, Descripcion = "Abono por Boleta"},
                new Deducciones{DeduccionId = 6, Descripcion = "Boleta Otras Deducciones"},
                new Deducciones{DeduccionId = 7, Descripcion = "Boleta Otros Ingresos"},
                new Deducciones{DeduccionId = 8, Descripcion = "Boleta con Humedad"},
                new Deducciones{DeduccionId = 9, Descripcion = "Boleta Deduccion Manual"},
                new Deducciones{DeduccionId = 10, Descripcion = "Ajuste de Boletas"}
            };

            MockSettingsFactory settingsFactory = new MockSettingsFactory();
            SettingFactory.SetCurrent(settingsFactory);
        }

        [TestMethod]
        public void Crear_boleta()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var mensajesValidacion = boleta.GetValidationErrors();

            //Asert
            Assert.AreEqual(0, mensajesValidacion.Count());
        }

        [TestMethod]
        public void Crear_boleta_con_bonificacion()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 6, 2, 45, 30, 0m, 4, 950, 1100, DateTime.Now, _tempImage, boletPath);
            List<BonificacionProducto> bonificacionesProducto = new List<BonificacionProducto>
            {
                new BonificacionProducto(gildan, aserrin, true),
                new BonificacionProducto(hgpc, aserrin, true)
            };

            //Acti
            bool canAssignBonus = _boletasDomainServices.CanAssignBonusProduct(boleta, bonificacionesProducto, out string errorMessage);

            //Asert
            Assert.AreEqual(true, canAssignBonus);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(19, boleta.PesoProducto);
        }

        [TestMethod]
        public void Crear_boleta_con_bonificacion_no_puede_ser_asignada_porque_no_existe_configuracion()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 6, 1, 45, 30, 0m, 4, 950, 1100, DateTime.Now, _tempImage, boletPath);
            List<BonificacionProducto> bonificacionesProducto = new List<BonificacionProducto>
            {
                new BonificacionProducto(gildan, aserrin, true),
                new BonificacionProducto(hgpc, aserrin, true)
            };

            //Acti
            bool canAssignBonus = _boletasDomainServices.CanAssignBonusProduct(boleta, bonificacionesProducto, out string errorMessage);

            //Asert
            Assert.AreEqual(false, canAssignBonus);
            Assert.AreEqual("El Producto no está configurado para Bonificaciones", errorMessage);
        }

        [TestMethod]
        public void Crear_boleta_con_bonificacion_no_puede_asignar_bonificacion_porque_la_configuracion_no_existe_actualizacion_boleta()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 6, 5, 45, 30, 0m, 4, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.BoletaId = 1;
            boleta.ClientePlanta = hgpc;
            boleta.CategoriaProducto = aserrin;

            List<BonificacionProducto> bonificacionesProducto = new List<BonificacionProducto>
            {
                new BonificacionProducto(gildan, aserrin, true),
                new BonificacionProducto(hgpc, aserrin, false)
            };

            //Acti
            bool canAssignBonus = _boletasDomainServices.CanAssignBonusProduct(boleta, bonificacionesProducto, out string errorMessage);

            //Asert
            Assert.AreEqual(false, canAssignBonus);
            Assert.AreEqual("El Producto Aserrin Nuevo está desactivado para Bonificaciones en HGPC", errorMessage);
        }

        [TestMethod]
        public void Crear_boleta_con_bonificacion_no_puede_asignar_bonificacion_porque_la_configuracion_esta_desactivada()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 6, 5, 45, 30, 0m, 4, 950, 1100, DateTime.Now, _tempImage, boletPath);
            List<BonificacionProducto> bonificacionesProducto = new List<BonificacionProducto>
            {
                new BonificacionProducto(gildan, aserrin, true),
                new BonificacionProducto(hgpc, aserrin, false)
            };

            //Acti
            bool canAssignBonus = _boletasDomainServices.CanAssignBonusProduct(boleta, bonificacionesProducto, out string errorMessage);

            //Asert
            Assert.AreEqual(false, canAssignBonus);
            Assert.AreEqual("El Producto está desactivado para Bonificaciones", errorMessage);
        }

        [TestMethod]
        public void Crear_boleta_con_bonificacion_no_puede_asignar_bonificacion_porque_la_configuracion_esta_desactivada_actualizacion_boleta()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 6, 5, 45, 30, 0m, 4, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.BoletaId = 1;
            boleta.ClientePlanta = hgpc;
            boleta.CategoriaProducto = aserrin;

            List<BonificacionProducto> bonificacionesProducto = new List<BonificacionProducto>
            {
                new BonificacionProducto(gildan, aserrin, true),
                new BonificacionProducto(hgpc, aserrin, false)
            };

            //Acti
            bool canAssignBonus = _boletasDomainServices.CanAssignBonusProduct(boleta, bonificacionesProducto, out string errorMessage);

            //Asert
            Assert.AreEqual(false, canAssignBonus);
            Assert.AreEqual("El Producto Aserrin Nuevo está desactivado para Bonificaciones en HGPC", errorMessage);
        }

        [TestMethod]
        public void Crear_boleta_no_puede_crear_sin_proveedor()
        {
            //Arange
            Proveedores vendorNull = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
            var boleta = new Boletas("Test", "TestEnvio", vendorNull, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var mensajesValidacion = boleta.GetValidationErrors();

            //Asert
            Assert.AreEqual("ProveedorId es requerido", mensajesValidacion.FirstOrDefault());
        }

        [TestMethod]
        public void Crear_boleta_no_puede_crearla_sin_numero_envio_cuando_la_planta_lo_requiere()
        {
            //Arange
            var boleta = new Boletas("Test", "", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var mensajesValidacion = boleta.GetValidationErrors();

            //Asert
            Assert.AreEqual("NumeroEnvio es requerido", mensajesValidacion.FirstOrDefault());
        }

        [TestMethod]
        public void Crear_boleta_puede_crearla_sin_numero_envio_cuando_la_planta_no_lo_requiere()
        {
            //Arange
            var boleta = new Boletas("Test", "", vendor, "Placa", "Motorista", 1, 4, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var mensajesValidacion = boleta.GetValidationErrors();

            //Asert
            Assert.AreEqual(0, mensajesValidacion.Count());
        }

        [TestMethod]
        public void Boleta_validar_totalPagar()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var totalPagar = boleta.ObtenerTotalAPagar();

            //Asert
            Assert.AreEqual(13764M, totalPagar);
        }

        [TestMethod]
        public void Boleta_validar_con_bonificacion_totalPagar()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0m, 5, 950, 1100, DateTime.Now, _tempImage, boletPath);

            //Acti
            var totalPagar = boleta.ObtenerTotalAPagar();
            decimal pesoProducto = boleta.PesoProducto;

            //Asert
            Assert.AreEqual(20, pesoProducto);
            Assert.AreEqual(18962M, totalPagar);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_solo_tasa_seguridad()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            
            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(30M, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_descarga_producto()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(1020M, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_orden_combustible()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };

            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(18430M, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_abonos_prestamo()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");

            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 6500, boleta.CodigoBoleta, "Segundo Abono")
            };

            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(11530M, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_otras_deducciones_negativas()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2")
            };

            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(4480M, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_todas_las_posibles_deducciones()
        {
            //Arange
            ClientePlantas facility = new ClientePlantas("RTN", "Gildan", "255", "Choloma");
            facility.PlantaId = 2;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 4000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Segundo Abono")
            };
            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -900, "Descarga Pendiente", null, null, null, true)
            };
            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            //Acti
            decimal totalDeducciones = boleta.ObtenerTotalDeduccion();

            //Asert
            Assert.AreEqual(33810, totalDeducciones);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_todas_las_posibles_deducciones_y_con_boleta_humedad()
        {
            //Arange
            ClientePlantas facility = new ClientePlantas("RTN", "Gildan", "255", "Choloma");
            facility.PlantaId = 2;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 4000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Segundo Abono")
            };
            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -900, "Descarga Pendiente", null, null, null, true)
            };
            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            BoletaHumedad boletaHumedad = new BoletaHumedad("TestEnvío", facility, false, 54.8m, 54, DateTime.Now);
            boletaHumedad.BoletaHumedadId = 1;
            
            BoletaHumedadPago boletaHumedadPago = new BoletaHumedadPago(boleta, boletaHumedad);
            boletaHumedadPago.BoletaHumedadPagoId = 1;

            BoletaHumedadAsignacion boletaHumedadAsignacion = new BoletaHumedadAsignacion(boleta, boletaHumedad);
            boletaHumedadAsignacion.BoletaHumedadAsignacionId = 1;

            boleta.AddBoletaHumidityPay(boletaHumedadPago);
            boletaHumedad.BoletaHumedadAsignacion = boletaHumedadAsignacion;
            boletaHumedad.BoletaHumedadPago = boletaHumedadPago;

            //TasaSeguridad = 71
            //Humedad = 277.20
            //Descarga 990
            //Diesel = 18,400
            //AbonosPrestamo = 9,000
            //BoletaOtrasDeducciones = 5,350

            //Acti
            decimal totalDeducciones = boleta.ObtenerTotalDeduccion();
            decimal totalPayment = boleta.ObtenerTotalAPagar();
            
            //Asert
            Assert.AreEqual(34088.2m, totalDeducciones);
            Assert.AreEqual(561.8m, totalPayment);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_todas_las_posibles_deducciones_y_con_ajuste_pagos()
        {
            //Arange
            ClientePlantas facility = new ClientePlantas("RTN", "Gildan", "255", "Choloma");
            facility.PlantaId = 2;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                BoletaId = 434
            };
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            Boletas boleta2 = new Boletas("Test2", "TestEnvio2", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                BoletaId = 3454
            };
            AjusteBoleta ajusteBoleta = new AjusteBoleta(boleta2);

            AjusteTipo ajusteKipper = new AjusteTipo("Kipper")
            {
                AjusteTipoId = 1
            };

            AjusteTipo ajusteDeposito = new AjusteTipo("Se deposito de más")
            {
                AjusteTipoId = 2
            };

            List<AjusteBoletaDetalle>  ajusteBoletaDetalles = new List<AjusteBoletaDetalle>
            {
                new AjusteBoletaDetalle(ajusteBoleta, ajusteKipper, 1000, null, null, "Kipper de la boleta #7854"),
                new AjusteBoletaDetalle(ajusteBoleta, ajusteDeposito, 2500.85m, _creditLine, "7855562", "Se deposito de más en la transferencia")
            };

            foreach (AjusteBoletaDetalle ajusteDetalle in ajusteBoletaDetalles)
            {
                ajusteDetalle.AjusteBoletaDetalleId++;
            }

            ajusteBoleta.AjusteBoletaDetalles = new HashSet<AjusteBoletaDetalle>(ajusteBoletaDetalles);

            AjusteBoletaPago ajustePago1 = new AjusteBoletaPago(ajusteBoletaDetalles.FirstOrDefault(), boleta, 500);
            ajustePago1.AjusteBoletaPagoId = 1;


            AjusteBoletaPago ajustePago2 = new AjusteBoletaPago(ajusteBoletaDetalles.Where(t => t.AjusteTipoId == 2).FirstOrDefault(), boleta, 60);
            ajustePago2.AjusteBoletaPagoId = 2;

            boleta.AddAjusteBoletaPago(ajustePago1);
            boleta.AddAjusteBoletaPago(ajustePago2);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 4000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Segundo Abono")
            };
            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -900, "Descarga Pendiente", null, null, null, true)
            };
            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            BoletaHumedad boletaHumedad = new BoletaHumedad("TestEnvío", facility, false, 54.8m, 54, DateTime.Now)
            {
                BoletaHumedadId = 1
            };

            BoletaHumedadPago boletaHumedadPago = new BoletaHumedadPago(boleta, boletaHumedad)
            {
                BoletaHumedadPagoId = 1
            };

            BoletaHumedadAsignacion boletaHumedadAsignacion = new BoletaHumedadAsignacion(boleta, boletaHumedad)
            {
                BoletaHumedadAsignacionId = 1
            };

            boleta.AddBoletaHumidityPay(boletaHumedadPago);
            boletaHumedad.BoletaHumedadAsignacion = boletaHumedadAsignacion;
            boletaHumedad.BoletaHumedadPago = boletaHumedadPago;
            
            //TasaSeguridad = 71
            //Humedad = 277.20
            //Descarga 990
            //Diesel = 18,400
            //AbonosPrestamo = 9,000
            //BoletaOtrasDeducciones = 5,350
            //AjusteBoletaPagos = 560

            //Acti
            decimal totalDeducciones = boleta.ObtenerTotalDeduccion();
            decimal totalPayment = boleta.ObtenerTotalAPagar();
            decimal ajusteBoletaAbonos = boleta.GetAjusteBoletaPayments();

            //Asert
            Assert.AreEqual(34648.2m, totalDeducciones);
            Assert.AreEqual(560m, ajusteBoletaAbonos);
            Assert.AreEqual(1.8m, totalPayment);
        }

        [TestMethod]
        public void Boleta_validar_totalDeduccion_con_todas_las_posibles_deducciones_y_con_boleta_humedad2()
        {
            //Arange
            ClientePlantas facility = new ClientePlantas("RTN", "Gildan", "255", "Choloma");
            facility.PlantaId = 2;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 4000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Segundo Abono")
            };
            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2")
            };
            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            BoletaHumedad boletaHumedad = new BoletaHumedad("TestEnvío", facility, false, 58.23m, 55, DateTime.Now);
            boletaHumedad.BoletaHumedadId = 1;

            BoletaHumedadPago boletaHumedadPago = new BoletaHumedadPago(boleta, boletaHumedad);
            boletaHumedadPago.BoletaHumedadPagoId = 1;

            BoletaHumedadAsignacion boletaHumedadAsignacion = new BoletaHumedadAsignacion(boleta, boletaHumedad);
            boletaHumedadAsignacion.BoletaHumedadAsignacionId = 1;

            boleta.AddBoletaHumidityPay(boletaHumedadPago);
            boletaHumedad.BoletaHumedadAsignacion = boletaHumedadAsignacion;
            boletaHumedad.BoletaHumedadPago = boletaHumedadPago;

            //TasaSeguridad = 71
            //Humedad = 1,118.7
            //Descarga 990
            //Diesel = 18,400
            //AbonosPrestamo = 9,000
            //BoletaOtrasDeducciones = 4,450

            //Acti
            decimal totalDeducciones = boleta.ObtenerTotalDeduccion();
            decimal totalPayment = boleta.ObtenerTotalAPagar();

            //Asert
            Assert.AreEqual(34029.70m, totalDeducciones);
            Assert.AreEqual(620.30m, totalPayment);
        }

        [TestMethod]
        public void Boleta_validar_totalPago_Deduccion_con_todas_las_posibles_deducciones_y_otros_ingresos()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 4000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Segundo Abono")
            };
            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            var otrasDeduccionesNegativas = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2")
            };
            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>(otrasDeduccionesNegativas);

            var otrosIngresos = new HashSet<BoletaOtrasDeducciones>
            {
                new BoletaOtrasDeducciones(boleta.BoletaId, 900, "Abono de boleta XXXX", "Interbanca", 234, "TestRef1"),
                new BoletaOtrasDeducciones(boleta.BoletaId, 1450, "Abono de boleta yyyyy", "Interbanca", 234, "TestRef1"),
            };

            foreach (var abono in otrosIngresos)
            {
                boleta.BoletaOtrasDeducciones.Add(abono);
            }

            //Acti
            var totalDeducciones = boleta.ObtenerTotalDeduccion();
            var totalPagar = boleta.ObtenerTotalAPagar();

            //Asert
            Assert.AreEqual(32910M, totalDeducciones);
            Assert.AreEqual(3614.80M, totalPagar);
        }

        [TestMethod]
        public void Eliminar_boleta_no_puede_eliminar_si_tiene_descarga_producto_y_este_diferente_ACTIVO()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);            
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);
            boleta.Descargador.PagoDescargador.Estado = Estados.ENPROCESO;

            //Acti
            string mensajeValidacion;
            _boletasDomainServices.TryEliminarBoleta(boleta, out mensajeValidacion);

            //Asert
            Assert.AreEqual("El Estado del Pago de la Descarga deber ser ACTIVO!", mensajeValidacion);
        }

        [TestMethod]
        public void Eliminar_boleta_asignado_a_factura()
        {
            //Arange
            ClientePlantas facility = new ClientePlantas("RTN", "Gildan", "255", "Choloma");
            facility.PlantaId = 2;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.BoletaId = 8574;

            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                FacturaCategoriaId = 1,
                Descripcion = "Crédito"
            };
            ClientePlantas planta = new ClientePlantas("RTN", "Gildan", "5541666", "Choloma");
            SubPlanta subPlanta;
            subPlanta = new SubPlanta(2, "Properties", "RTN", "choloma", true, "Registro");
            subPlanta.Planta = planta;
            subPlanta.SubPlantaId = 1;
            Factura invoice = new Factura(2, invoiceCategory, planta, subPlanta.SubPlantaId, "PO", string.Empty, "1258", "ProForma", DateTime.Now, 151785.85m,
                                  "ExonerationNo", 0m, true, 378596.26m, "test");
            FacturaDetalleBoletas facturaDetalleBoleta = new FacturaDetalleBoletas(invoice, null, "97895", string.Empty, 40.63m, DateTime.Now);
            facturaDetalleBoleta.FacturaDetalleBoletaId = 1;
            boleta.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            boleta.FacturaDetalleBoletas.Add(facturaDetalleBoleta);

            //Acti
            _boletasDomainServices.TryEliminarBoleta(boleta, out string mensajeValidacion);

            //Asert
            Assert.AreEqual(string.Empty, mensajeValidacion);
            Assert.AreEqual(null, facturaDetalleBoleta.BoletaId);
        }

        [TestMethod]
        public void Eliminar_boleta_no_puede_eliminar_si_tiene_abono_prestamo_y_este_diferente_ACTIVO()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 5500, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.PagoPrestamos.FirstOrDefault().Prestamo.Estado = Estados.CERRADO;

            //Acti
            string mensajeValidacion;
            _boletasDomainServices.TryEliminarBoleta(boleta, out mensajeValidacion);

            //Asert
            Assert.AreEqual(string.Format("Ya está CERRADO el Préstamo {0}", prestamo.CodigoPrestamo), mensajeValidacion);
        }

        [TestMethod]
        public void Eliminar_boleta_con_asignacion_de_orden_de_diesel()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ENPROCESO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Acti
            _boletasDomainServices.TryEliminarBoleta(boleta, out string mensajeValidacion);

            //Asert
            Assert.AreEqual(string.Empty, mensajeValidacion);
        }

        [TestMethod]
        public void Eliminar_boleta_no_puede_eliminar_si_tiene_otras_deducciones_y_la_linea_credito_de_estos_diferente_ACTIVO()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.CERRADO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, 3500, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;
            //Acti
            string mensajeValidacion;
            _boletasDomainServices.TryEliminarBoleta(boleta, out mensajeValidacion);

            //Asert
            Assert.AreEqual(string.Format("Ya no puede Eliminar Deducciones de la Linea de Crédito {0} porque está {1}", lineaCredito.CodigoLineaCredito, lineaCredito.Estado), mensajeValidacion);
        }

        [TestMethod]
        public void Eliminar_boleta_no_puede_eliminar_si_tiene_descarga_producto_por_adelantado_ACTIVO()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 5500, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.PagoPrestamos.FirstOrDefault().Prestamo.Estado = Estados.ACTIVO;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, 3500, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //Acti
            string mensajeValidacion;
            var puedeEliminarBoleta = _boletasDomainServices.TryEliminarBoleta(boleta, out mensajeValidacion);

            //Asert
            Assert.AreEqual(true, puedeEliminarBoleta);
            Assert.AreEqual("", mensajeValidacion);
        }

        [TestMethod]
        public void TieneMoraDePrestamo()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            var boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            proveedor.Prestamos.Add(prestamo);
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 5500, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.PagoPrestamos.FirstOrDefault().Prestamo.Estado = Estados.ACTIVO;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, 3500, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //Acti
            var clientHasLoan = boleta.ClientHasLoan();

            //Asert
            Assert.AreEqual(true, clientHasLoan);
        }

        [TestMethod]
        public void NoTieneMoraDePrestamo()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            var boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            proveedor.Prestamos.Add(prestamo);
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 20000, boleta.CodigoBoleta, "Testing");            
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.PagoPrestamos.FirstOrDefault().Prestamo.Estado = Estados.CERRADO;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, 3500, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //Acti
            var clientHasLoan = boleta.ClientHasLoan();

            //Asert
            Assert.AreEqual(false, clientHasLoan);
        }

        [TestMethod]
        public void AplicaCierreForzado()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            var boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            proveedor.Prestamos.Add(prestamo);
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 20000, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);
            
            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, -4214.80m, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //Acti
            var saldoPendiente = boleta.ObtenerTotalAPagar();
            var clientHasLoan = boleta.AppliesToForceClosing();


            //Asert
            Assert.AreEqual(true, clientHasLoan);
            Assert.AreEqual(0, saldoPendiente);
        }

        [TestMethod]
        public void NoAplicaCierreForzado()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            var boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            proveedor.Prestamos.Add(prestamo);
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 20000, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);
            
            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, -4214.0m, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //act
            var saldoPendiente = boleta.ObtenerTotalAPagar();
            var clientHasLoan = boleta.AppliesToForceClosing();
            
            //Asert
            Assert.AreEqual(false, clientHasLoan);
            Assert.AreEqual(0.80m, saldoPendiente);
        }

        [TestMethod]
        public void Eliminar_boleta_pasa_todas_las_validaciones()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);

            //Abonos
            var prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 20000, "Testing");
            var abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 5500, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.PagoPrestamos.FirstOrDefault().Prestamo.Estado = Estados.ACTIVO;

            //Orden Combustible
            var ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Otras Deducciones
            var lineaCredito = new LineasCredito("LineaCreditoTest", 1, 1, "Manual", "Deposito de saldos", DateTime.Now, 50000, 0, "Josue Cruz");
            lineaCredito.Estado = Estados.ACTIVO;
            var boletaOtrasDeducciones = new BoletaOtrasDeducciones(boleta.BoletaId, 3500, "Testing", "Manual", lineaCredito.LineaCreditoId, "123");
            boleta.AgregarOtraDeduccion(boletaOtrasDeducciones);
            boleta.BoletaOtrasDeducciones.FirstOrDefault().LineasCredito = lineaCredito;

            //Acti
            string mensajeValidacion;
            var puedeEliminarBoleta = _boletasDomainServices.TryEliminarBoleta(boleta, out mensajeValidacion);

            //Asert
            Assert.AreEqual(true, puedeEliminarBoleta);
            Assert.AreEqual("", mensajeValidacion);
        }

        [TestMethod]
        public void TryOpenBoletaById_solo_puede_abrir_boletas_cerradas()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            proveedor.IsExempt = true;
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;
            boleta.BoletaDetalles = new HashSet<BoletaDetalles>();

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 1, boleta.Descargador.PrecioDescarga, string.Empty, "Descargado por Alexis"));

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 2, ordenCombustible.Monto, "Fact #78545", "Diesel"));

            //Abonos
            Prestamos prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 24284.7m, "Testing");
            proveedor.Prestamos.Add(prestamo);
            PagoPrestamos abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 24284.7m, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 3, abonoPrestamo.MontoAbono, "Abono PrestamoTest", string.Empty));

            //Cierre de Prestamo y Boleta
            //boleta.Estado = Estados.CERRADO;
            //prestamo.Estado = Estados.CERRADO;

            //Act
            string errorMessage;
            bool canOpen = _boletasDomainServices.TryOpenBoletaById(boleta, out errorMessage);

            //Asert
            Assert.AreEqual("El estado de la Boleta debe ser CERRADO!",errorMessage);
            Assert.AreEqual(false, canOpen);
        }

        [TestMethod]
        public void TryCierreForzado_escenario_satisfactorio_abono_total_a_ajuste_de_boleta()
        {
            //Arange
            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com")
            {
                ProveedorId = 1045,
                IsExempt = true
            };
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                Proveedor = proveedor,
                BoletaDetalles = new HashSet<BoletaDetalles>()
            };

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false)
            {
                PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now)
            };

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            {
                Estado = Estados.ACTIVO,
                Boleta = boleta
            };
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Ajuste Boleta
            Boletas boleta2 = new Boletas("Test2", "TestEnvio2", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                BoletaId = 3454
            };
            AjusteBoleta ajusteBoleta = new AjusteBoleta(boleta2);

            AjusteTipo ajusteKipper = new AjusteTipo("Kipper")
            {
                AjusteTipoId = 1
            };

            AjusteTipo ajusteDeposito = new AjusteTipo("Se deposito de más")
            {
                AjusteTipoId = 2
            };

            List<AjusteBoletaDetalle> ajusteBoletaDetalles = new List<AjusteBoletaDetalle>
            {
                new AjusteBoletaDetalle(ajusteBoleta, ajusteKipper, 1000, null, null, "Kipper de la boleta #7854"),
                new AjusteBoletaDetalle(ajusteBoleta, ajusteDeposito, 2500.85m, _creditLine, "7855562", "Se deposito de más en la transferencia")
            };

            foreach (AjusteBoletaDetalle ajusteDetalle in ajusteBoletaDetalles)
            {
                ajusteDetalle.AjusteBoletaDetalleId++;
            }

            ajusteBoleta.AjusteBoletaDetalles = new HashSet<AjusteBoletaDetalle>(ajusteBoletaDetalles);

            AjusteBoletaPago ajustePago1 = new AjusteBoletaPago(ajusteBoletaDetalles.FirstOrDefault(), boleta, 500)
            {
                AjusteBoletaPagoId = 1
            };


            AjusteBoletaPago ajustePago2 = new AjusteBoletaPago(ajusteBoletaDetalles.Where(t => t.AjusteTipoId == 2).FirstOrDefault(), boleta, 23784.7m)
            {
                AjusteBoletaPagoId = 2
            };


            AjusteBoletaDetalle ajustmentKipper = ajusteBoleta.AjusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.AjusteTipoId == 1);
            ajustmentKipper.AjusteBoletaPagos.Add(ajustePago1);

            AjusteBoletaDetalle ajustmentDepositoDeMas = ajusteBoleta.AjusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.AjusteTipoId == 2);
            ajustmentDepositoDeMas.AjusteBoletaPagos.Add(ajustePago2);

            boleta.AddAjusteBoletaPago(ajustePago1);
            boleta.AddAjusteBoletaPago(ajustePago2);


            //Act
            bool canClose = _boletasDomainServices.TryCierreForzado(boleta, _deductions, out string errorMessage);

            //Asert
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(true, canClose);
            Assert.AreEqual(Estados.CERRADO, boleta.Estado);
            Assert.AreEqual(5, boleta.BoletaDetalles.Count);
            Assert.AreEqual(Estados.ENPROCESO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryCierreForzado_escenario_satisfactorio_abono_completo_a_prestamo()
        {
            //Arange
            LineasCredito creditLine = new LineasCredito("LC20-00035", 1, 34, "FT20207845PWK", "Deposito de saldos", DateTime.Now, 120000, 0, "ecruz")
            {
                LineaCreditoId = 45,
                Estado = Estados.ACTIVO
            };

            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com")
            {
                ProveedorId = 1045,
                IsExempt = true
            };
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                Proveedor = proveedor,
                BoletaDetalles = new HashSet<BoletaDetalles>()
            };

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false)
            {
                PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now)
            };

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            {
                Estado = Estados.ACTIVO,
                Boleta = boleta
            };
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Abonos
            Prestamos prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 24284.7m, "Testing");
            prestamo.PrestamosTransferencias = new List<PrestamosTransferencias>
            {
                new  PrestamosTransferencias(prestamo.PrestamoId, "Interbanca", creditLine.LineaCreditoId, "REF:855746", 24284.7m)
            };
            proveedor.AddNewLoan(prestamo);
            prestamo.Activar();
            PagoPrestamos abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 24284.7m, boleta.CodigoBoleta, "Testing");
            prestamo.AgregarOtroAbono(abonoPrestamo);
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;

            //Act
            bool canClose = _boletasDomainServices.TryCierreForzado(boleta, _deductions, out string errorMessage);

            //Asert
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(true, canClose);
            Assert.AreEqual(Estados.CERRADO, boleta.Estado);
            Assert.AreEqual(4, boleta.BoletaDetalles.Count);
            Assert.AreEqual(Estados.CERRADO, prestamo.Estado);
        }

        [TestMethod]
        public void TryOpenBoletaById_se_activa_satisfactoriamente_boleta_abonada_completamente()
        {
            //Arange
            LineasCredito creditLine = new LineasCredito("LC20-00035", 1, 34, "FT20207845PWK", "Deposito de saldos", DateTime.Now, 120000, 0, "ecruz")
            {
                LineaCreditoId = 45,
                Estado = Estados.ACTIVO
            };

            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com")
            {
                ProveedorId = 1045,
                IsExempt = true
            };
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                Proveedor = proveedor,
                BoletaDetalles = new HashSet<BoletaDetalles>()
            };

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false)
            {
                PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now)
            };

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            {
                Estado = Estados.ACTIVO,
                Boleta = boleta
            };
            boleta.AgregarOrdenCombustible(ordenCombustible);

            //Abonos
            Prestamos prestamo = new Prestamos("PrestanoTest", 1, 1, DateTime.Now, "Josue Cruz", 0, 24284.7m, "Testing");
            prestamo.PrestamosTransferencias = new List<PrestamosTransferencias>
            {
                new  PrestamosTransferencias(prestamo.PrestamoId, "Interbanca", creditLine.LineaCreditoId, "REF:855746", 24284.7m)
            };
            proveedor.AddNewLoan(prestamo);
            prestamo.Activar();
            PagoPrestamos abonoPrestamo = new PagoPrestamos(prestamo.PrestamoId, "Abobno por Boleta", null, boleta.BoletaId, 24284.7m, boleta.CodigoBoleta, "Testing");
            boleta.AgregarAbonoPrestamo(abonoPrestamo);
            boleta.PagoPrestamos.FirstOrDefault().Prestamo = prestamo;

            //Actualziacion de estado Boleta y prestamo
            _boletasDomainServices.TryCierreForzado(boleta, _deductions, out string errorMessage);

            //Act
            bool canOpen = _boletasDomainServices.TryOpenBoletaById(boleta, out errorMessage);

            //Asert
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(true, canOpen);
            Assert.AreEqual(Estados.ACTIVO, boleta.Estado);
            Assert.AreEqual(0, boleta.BoletaDetalles.Count);
            Assert.AreEqual(Estados.ACTIVO, prestamo.Estado);
        }

        [TestMethod]
        public void TryOpenBoletaById_se_activa_satisfactoriamente_boleta_con_deposito_de_pago()
        {
            //Arange
            LineasCredito creditLine = new LineasCredito("LC20-00035", 1, 34, "FT20207845PWK", "Deposito de saldos", DateTime.Now, 120000, 0, "ecruz")
            {
                LineaCreditoId = 45,
                Estado = Estados.ACTIVO
            };

            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com")
            {
                ProveedorId = 1045,
                IsExempt = true
            };
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath)
            {
                Proveedor = proveedor,
                BoletaDetalles = new HashSet<BoletaDetalles>()
            };

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false)
            {
                PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now)
            };
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 1, boleta.Descargador.PrecioDescarga, string.Empty, "Descargado por Alexis"));

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            {
                Estado = Estados.ACTIVO,
                Boleta = boleta
            };
            boleta.AgregarOrdenCombustible(ordenCombustible);
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 2, ordenCombustible.Monto, "Fact #78545", "Diesel"));

            BoletaCierres boletaCierre = new BoletaCierres(boleta.BoletaId, "Interbanca", creditLine.LineaCreditoId, "4526", 24284.70m, DateTime.Now)
            {
                LineasCredito = creditLine
            };
            boleta.BoletaCierres = new HashSet<BoletaCierres> { boletaCierre };

            boleta.ActualizarEstadoBoleta();

            //Act
            bool canOpen = _boletasDomainServices.TryOpenBoletaById(boleta, out string errorMessage);

            //Asert
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(true, canOpen);
            Assert.AreEqual(Estados.ACTIVO, boleta.Estado);
            Assert.AreEqual(0, boleta.BoletaDetalles.Count);
        }

        [TestMethod]
        public void TryOpenBoletaById_no_puede_abrir_con_linea_de_credito_cerrada()
        {
            //Arrange
            LineasCredito creditLine = new LineasCredito("LC20-00035", 1, 34, "FT20207845PWK", "Deposito de saldos", DateTime.Now, 120000, 0, "ecruz")
            {
                LineaCreditoId = 45,
                Estado = Estados.ACTIVO
            };

            Proveedores proveedor = new Proveedores("Rtn", "Josue Cruz", "0501-1992-00882", "direccion", "9445-8425", "ecruzj@outlook.com");
            proveedor.ProveedorId = 1045;
            proveedor.IsExempt = true;
            Boletas boleta = new Boletas("Test", "TestEnvio", proveedor, "Placa", "Motorista", 1, 1, 45, 10, 0.48m, 0, 990, 1100, DateTime.Now, _tempImage, boletPath);
            boleta.Proveedor = proveedor;
            boleta.BoletaDetalles = new HashSet<BoletaDetalles>();

            //Descarga de Producto
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.PagoDescargador = new PagoDescargadores("TestPagoDescarga", 1, "Josue Cruz", DateTime.Now);
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 1, boleta.Descargador.PrecioDescarga, string.Empty, "Descargado por Alexis"));

            //Orden Combustible
            OrdenesCombustible ordenCombustible = new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId);
            ordenCombustible.Estado = Estados.ACTIVO;
            ordenCombustible.Boleta = boleta;
            boleta.AgregarOrdenCombustible(ordenCombustible);
            boleta.AddBoletaDetalle(CreateBoletaDetalle(boleta, 2, ordenCombustible.Monto, "Fact #78545", "Diesel"));

            BoletaCierres boletaCierre = new BoletaCierres(boleta.BoletaId, "Interbanca", creditLine.LineaCreditoId, "4526", 24284.70m, DateTime.Now);
            boletaCierre.LineasCredito = creditLine;
            boleta.BoletaCierres = new HashSet<BoletaCierres> { boletaCierre };

            boleta.ActualizarEstadoBoleta();
            creditLine.Estado = Estados.CERRADO;

            //Act
            _boletasDomainServices.TryOpenBoletaById(boleta, out string errorMessage);

            //Assert
            Assert.AreEqual($"La linea de crédito {creditLine.CodigoLineaCredito} está cerrada, contactarse con el administrador", errorMessage);
            Assert.AreEqual(Estados.CERRADO, boleta.Estado);
        }

        private BoletaDetalles CreateBoletaDetalle(Boletas boleta, int tipoDeduccion, decimal montoDeduccion, string docRef, string observaciones)
        {
            return new BoletaDetalles(boleta.BoletaId, tipoDeduccion, montoDeduccion, docRef, observaciones);
        }
    }
}
