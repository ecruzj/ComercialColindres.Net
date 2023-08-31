using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Test.UnitTests.DomainServicesTests
{
    [TestClass]
    public class DescargadoresDomainServicesTests
    {
        readonly string BoletaPath = "";
        private IDescargadoresDomainServices _descargadoresDomainServices;
        Boletas _miBoleta;
        Proveedores _vendor;
        ClientePlantas _miPlanta;
        Cuadrillas _miCuadrilla;
        PagoDescargadores _pagoDescargadores;
        Descargadores _descargaProducto;
        AjusteTipo _ajusteDescarga;
        byte[] _tempImage;
        DescargasPorAdelantado _descargaAdelanto;
        private LineasCredito _creditLine;

        private LineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;
        private IAjusteBoletaDomainServices _ajusteBoletaDomainServices;

        [TestInitialize]
        public void Inicializacion()
        { 
            _lineasCreditoDeduccionesDomainServices = new LineasCreditoDeduccionesDomainServices();
            _tempImage = new byte[7];
            _ajusteBoletaDomainServices = new AjusteBoletaDomainServices(_lineasCreditoDeduccionesDomainServices);
            _descargadoresDomainServices = new DescargadoresDomainServices(_ajusteBoletaDomainServices);
            _creditLine = new LineasCredito("LC21-00258", 1, 1, "FT202185485BTY", "Transferencia de Ficohsa", DateTime.Now, 150000, 7800, "ecruzj")
            {
                LineaCreditoId = 78
            };

            _vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com")
            {
                ProveedorId = 1
            };

            _miBoleta = new Boletas("Test", "TestEnvio", _vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, BoletaPath)
            {
                BoletaId = 5874,
                Estado = Estados.ACTIVO
            };

            _miPlanta = new ClientePlantas("Planta RTN", "Gildan", "2554", "Choloma, Cortes")
            {
                PlantaId = 8
            };

            _miCuadrilla = new Cuadrillas("Cuadrilla Test", _miPlanta.PlantaId, true)
            {
                CuadrillaId = 10
            };

            _pagoDescargadores = new PagoDescargadores("PG18-0002", _miCuadrilla.CuadrillaId, "Josue Cruz", DateTime.Now)
            {
                PagoDescargaId = 8745,
                Cuadrilla = _miCuadrilla
            };

            _descargaProducto = new Descargadores(_miBoleta.BoletaId, _miCuadrilla.CuadrillaId, 1450, DateTime.Now, false)
            {
                DescargadaId = 1045
            };

            _ajusteDescarga = new AjusteTipo("Descarga")
            {
                AjusteTipoId = 2
            };

            _descargaAdelanto = new DescargasPorAdelantado(string.Empty, string.Empty, _pagoDescargadores, 0m, DateTime.Now, true);

            MockSettingsFactory settingsFactory = new MockSettingsFactory();
            SettingFactory.SetCurrent(settingsFactory);
        }

        [TestMethod]
        public void TryCrearDescargaProducto_Puede_Asignar_Descarga_de_Producto()
        {
            //Arange
            HashSet<DescargasPorAdelantado> descargasPorAdelanto = new HashSet<DescargasPorAdelantado>();

            var descargaPorAdelantado1 = new DescargasPorAdelantado(_miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, _pagoDescargadores, 1300, DateTime.Now)
            {
                PagoDescargador = _pagoDescargadores,
                DescargaPorAdelantadoId = 1
            };
            descargasPorAdelanto.Add(descargaPorAdelantado1);

            var descargaPorAdelantado2 = new DescargasPorAdelantado("78554", "966635", _pagoDescargadores, 1300, DateTime.Now)
            {
                PagoDescargador = _pagoDescargadores,
                DescargaPorAdelantadoId = 2
            };
            descargasPorAdelanto.Add(descargaPorAdelantado2);

            var descargaPorAdelantado3 = new DescargasPorAdelantado("78544", "966638", _pagoDescargadores, 1300, DateTime.Now)
            {
                PagoDescargador = _pagoDescargadores,
                DescargaPorAdelantadoId = 3
            };
            descargasPorAdelanto.Add(descargaPorAdelantado3);

            //Act
            _descargadoresDomainServices.TryAsignarDescargaProductoPorAdelantado(_miBoleta, descargasPorAdelanto.ToList());

            //Assert
            Assert.AreEqual(Estados.ASIGNADO, descargaPorAdelantado1.Estado);
            Assert.IsNotNull(_miBoleta.Descargador);
        }

        [TestMethod]
        public void TryCrearDescargaProducto_No_Asigna_Descarga_de_Producto()
        {
            //Arange

            //Act
            _descargadoresDomainServices.TryAsignarDescargaProductoPorAdelantado(_miBoleta, null);

            //Assert
            Assert.IsNull(_miBoleta.Descargador);
        }

        [TestMethod]
        public void TryActualizarPrecioDescargaProducto_Puede_Actualizar_Precio()
        {
            //Arange
            _miBoleta.Descargador = _descargaProducto;

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.TryActualizarPrecioDescargaProducto(_miBoleta, 1100, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(true, puedeActualizarPrecio);
            Assert.AreEqual(string.Empty, mensajeValidacion);
            Assert.AreEqual(1100, _miBoleta.Descargador.PrecioDescarga);
        }

        [TestMethod]
        public void TryActualizarPrecioDescargaProducto_No_Puede_Actualizar_Precio_la_boleta_debe_estar_Activa()
        {
            //Arange
            _miBoleta.Estado = Estados.CERRADO;
            _miBoleta.Descargador = _descargaProducto;

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.TryActualizarPrecioDescargaProducto(_miBoleta, 1100, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(false, puedeActualizarPrecio);
            Assert.AreEqual("El Estado de la Boleta debe ser ACTIVO", mensajeValidacion);
            Assert.AreEqual(1450, _miBoleta.Descargador.PrecioDescarga);
        }

        [TestMethod]
        public void TryActualizarPrecioDescargaProducto_No_Puede_el_precio_de_la_descarga_debe_ser_mayor_a_0()
        {
            //Arange
            _miBoleta.Descargador = _descargaProducto;

            //Act
            var mensajeValidacion = string.Empty;
            var puedeActualizarPrecio = _descargadoresDomainServices.TryActualizarPrecioDescargaProducto(_miBoleta, 0, out mensajeValidacion);

            //Assert
            Assert.AreEqual(false, puedeActualizarPrecio);
            Assert.AreEqual("El Precio de la descarga debe ser mayor a 0", mensajeValidacion);
            Assert.AreEqual(1450, _miBoleta.Descargador.PrecioDescarga);
        }

        [TestMethod]
        public void TryActualizarPrecioDescargaProducto_No_Puede_debe_tener_una_descarga_Asignada_a_la_boleta()
        {
            //Arange

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.TryActualizarPrecioDescargaProducto(_miBoleta, 1200, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(false, puedeActualizarPrecio);
            Assert.AreEqual("No existe Descarga de Producto Asignada a la Boleta", mensajeValidacion);
        }

        [TestMethod]
        public void TryActualizarPrecioDescargaProducto_No_Puede_Actualizar_el_pago_de_Descargas_debe_ser_Activo_en_caso_de_tenerlo()
        {
            //Arange
            _miBoleta.Descargador = _descargaProducto;
            _pagoDescargadores.Estado = Estados.ENPROCESO;
            _descargaProducto.PagoDescargador = _pagoDescargadores;

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.TryActualizarPrecioDescargaProducto(_miBoleta, 1100, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(false, puedeActualizarPrecio);
            Assert.AreEqual("El Estado de la Orden de Pago del Descargador debe estar ACTIVO", mensajeValidacion);
            Assert.AreEqual(1450, _miBoleta.Descargador.PrecioDescarga);
        }

        [TestMethod]
        public void PuedeEliminarDescargaProducto_no_puede_eliminar_descarga_si_ya_esta_en_proceso_de_pago()
        {
            //Arange
            _miBoleta.Descargador = _descargaProducto;
            _pagoDescargadores.Estado = Estados.ENPROCESO;
            _descargaProducto.PagoDescargador = _pagoDescargadores;

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.PuedeEliminarDescargaProducto(_descargaProducto, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(false, puedeActualizarPrecio);
            Assert.AreEqual("El Estado del Pago de la Descarga deber ser ACTIVO!", mensajeValidacion);
            Assert.AreEqual(1450, _miBoleta.Descargador.PrecioDescarga);
        }

        [TestMethod]
        public void PuedeEliminarDescargaProducto_escenario_ideal()
        {
            //Arange
            _miBoleta.Descargador = _descargaProducto;
            _descargaProducto.PagoDescargador = _pagoDescargadores;

            //Act
            var puedeActualizarPrecio = _descargadoresDomainServices.PuedeEliminarDescargaProducto(_descargaProducto, out string mensajeValidacion);

            //Assert
            Assert.AreEqual(true, puedeActualizarPrecio);
            Assert.AreEqual(string.Empty, mensajeValidacion);
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_AssigneDescargaToPay_Successful()
        {
            //Arrange
            _miBoleta.Descargador = _descargaProducto;

            //Act
            _descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, _miBoleta, _ajusteDescarga, _descargaAdelanto, _miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, 1450m, out string errorMessage);
            decimal totalPago = _pagoDescargadores.ObtenerTotalPago();

            //Assert
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(1450m, totalPago);
            Assert.AreEqual(_descargaProducto.PagoDescargaId, _pagoDescargadores.PagoDescargaId);
            Assert.AreEqual(Estados.ENPROCESO, _miBoleta.Descargador.Estado);
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_AssigneDescargaToPay_no_puede_la_descarga_ya_pertenece_a_otro_pago()
        {
            //Arrange
            PagoDescargadores pagoDescarga = new PagoDescargadores("PD20-00254", 1, "Josue Cruz", DateTime.Now);
            pagoDescarga.PagoDescargaId = 758;
            pagoDescarga.AddDescargaToPayment(_descargaProducto, _descargaProducto.PrecioDescarga);
            _descargaProducto.PagoDescargador = pagoDescarga;
            _miBoleta.Descargador = _descargaProducto;

            //Act
            bool canAssigne =_descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, _miBoleta, _ajusteDescarga, _descargaAdelanto, _miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, 1450m, out string errorMessage);

            //Assert
            Assert.AreEqual(false, canAssigne);
            Assert.AreEqual($"La descarga ya pertenece al pago {pagoDescarga.CodigoPagoDescarga}", errorMessage);
            Assert.AreEqual(_descargaProducto.PagoDescargaId, pagoDescarga.PagoDescargaId);
            Assert.AreEqual(Estados.ENPROCESO, _miBoleta.Descargador.Estado);
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_AssigneDescargaToPay_debe_crear_un_ajuste_por_no_cobrarle_al_proveedor_la_descarga_completa()
        {
            //Arrange
            _miBoleta.Descargador = _descargaProducto;

            //Act
            bool canAssigne = _descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, _miBoleta, _ajusteDescarga, _descargaAdelanto, _miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, 1500m, out string errorMessage);
            bool hasAjusteBoleta = _miBoleta.HasAjusteBoleta();
            AjusteBoleta ajusteBoleta = _miBoleta.GetAjusteBoleta();

            //Assert
            Assert.AreEqual(true, canAssigne);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(_descargaProducto.PagoDescargaId, _pagoDescargadores.PagoDescargaId);
            Assert.AreEqual(Estados.ENPROCESO, _miBoleta.Descargador.Estado);
            Assert.AreEqual(true, hasAjusteBoleta);
            Assert.AreEqual(Estados.ACTIVO, ajusteBoleta.Estado);
            Assert.AreEqual("No se cobro la descarga completa", ajusteBoleta.AjusteBoletaDetalles.FirstOrDefault().Observaciones);
            Assert.AreEqual(50, ajusteBoleta.GetAjusteBoletaTotal());
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_AssigneDescargaToPay_debe_crear_una_descarga_manual_y_un_ajuste_por_no_cobrarle_al_proveedor_la_descarga()
        {
            //Arrange

            //Act
            bool canAssigne = _descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, _miBoleta, _ajusteDescarga, _descargaAdelanto, _miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, 1000m, out string errorMessage);
            bool hasAjusteBoleta = _miBoleta.HasAjusteBoleta();
            AjusteBoleta ajusteBoleta = _miBoleta.GetAjusteBoleta();

            //Assert
            Assert.AreEqual(true, canAssigne);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(_miBoleta.Descargador.PagoDescargaId, _pagoDescargadores.PagoDescargaId);
            Assert.AreEqual(true, _miBoleta.Descargador.EsIngresoManual);
            Assert.AreEqual(Estados.ENPROCESO, _miBoleta.Descargador.Estado);
            Assert.AreEqual(true, hasAjusteBoleta);
            Assert.AreEqual(Estados.ACTIVO, ajusteBoleta.Estado);
            Assert.AreEqual("Se olvido cobrar la descarga", ajusteBoleta.AjusteBoletaDetalles.FirstOrDefault().Observaciones);
            Assert.AreEqual(1000, ajusteBoleta.GetAjusteBoletaTotal());
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_AssigneDescargaToPay_debe_crear_una_descarga_por_adelantado()
        {
            //Arrange
            _pagoDescargadores.Cuadrilla.ClientePlanta = _miPlanta;
            //Act
            bool canAssigne = _descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, null, _ajusteDescarga, null, _miBoleta.NumeroEnvio, _miBoleta.CodigoBoleta, 1000m, out string errorMessage);
            DescargasPorAdelantado descargaAdelanto = _pagoDescargadores.DescargasPorAdelantado.FirstOrDefault();

            //Assert
            Assert.AreEqual(true, canAssigne);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual("TestEnvio", descargaAdelanto.NumeroEnvio);
            Assert.AreEqual(1000, _pagoDescargadores.ObtenerTotalPago());
        }

        [TestMethod]
        public void TryAssigneDescargaToPay_No_puede_crear_descarga_por_adelanto_si_este_ya_existe()
        {
            //Arrange
            PagoDescargadores pagoDescarga = new PagoDescargadores("PagoTest", _miCuadrilla.CuadrillaId, "ecruzj", DateTime.Now);
            pagoDescarga.PagoDescargaId = 105;

            _descargaAdelanto.DescargaPorAdelantadoId = 50;
            _descargaAdelanto.CodigoBoleta = "47885";
            _descargaAdelanto.PrecioDescarga = 1300;
            _descargaAdelanto.PlantaId = 5;
            _descargaAdelanto.PagoDescargador = pagoDescarga;
            _descargaAdelanto.PagoDescargaId = pagoDescarga.PagoDescargaId;
            
            _pagoDescargadores.Cuadrilla.ClientePlanta = _miPlanta;
            _pagoDescargadores.DescargasPorAdelantado.Add(_descargaAdelanto);

            //Act
            bool canAssigne = _descargadoresDomainServices.TryAssigneDescargaToPay(_pagoDescargadores, null, _ajusteDescarga, _descargaAdelanto, _miBoleta.NumeroEnvio, "47885", 1000m, out string errorMessage);

            //Assert
            Assert.AreEqual(false, canAssigne);
            Assert.AreEqual($"La Descarga 47885 ya pertenece al pago de Adeltanto { _descargaAdelanto.PagoDescargador.CodigoPagoDescarga}", errorMessage);
        }
    }
}
