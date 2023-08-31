using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.DomainServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ComercialColindres.Test.UnitTests.DomainEntitiesTests
{
    [TestClass]
    public class AjusteBoletaTests
    {
        private IAjusteBoletaDomainServices _ajusteBoletaDomainServices;
        Proveedores _vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        Boletas _boleta;
        AjusteBoleta ajusteBoleta;
        AjusteTipo _ajusteKipper;
        AjusteTipo _ajusteDeposito;
        AjusteTipo _ajusteToneladas;
        List<AjusteBoletaDetalle> _ajusteBoletaDetalles;
        readonly Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        byte[] _tempImage;
        readonly string boletPath = "";
        readonly ILineasCreditoDeduccionesDomainServices _lineasCreditoDeduccionesDomainServices;
        private LineasCredito _creditLine;

        [TestInitialize]
        public void Initialization()
        {
            _tempImage = new byte[7];
            _creditLine = new LineasCredito("LC21-00258", 1, 1, "FT202185485BTY", "Transferencia de Ficohsa", DateTime.Now, 150000, 7800, "ecruzj")
            {
                LineaCreditoId = 78
            };

            _ajusteBoletaDomainServices = new AjusteBoletaDomainServices(_lineasCreditoDeduccionesDomainServices);
            _boleta = new Boletas("Test", "TestEnvio", _vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath)
            {
                BoletaId = 14
            };

            _ajusteKipper = new AjusteTipo("Kipper")
            {
                AjusteTipoId = 1
            };

            _ajusteDeposito = new AjusteTipo("Se deposito de más")
            {
                AjusteTipoId = 2
            };

            _ajusteToneladas = new AjusteTipo("Se pago 2 toneladas de más")
            {
                AjusteTipoId = 3
            };

            ajusteBoleta = new AjusteBoleta(_boleta);

            _ajusteBoletaDetalles = new List<AjusteBoletaDetalle>
            {
                new AjusteBoletaDetalle(ajusteBoleta, _ajusteKipper, 1000, _creditLine, "7855562", "Kipper de la boleta #7854"),
                new AjusteBoletaDetalle(ajusteBoleta, _ajusteDeposito, 2500.85m, _creditLine, "7855578", "Se deposito de más en la transferencia"),
                new AjusteBoletaDetalle(ajusteBoleta, _ajusteToneladas, 1750.98m, _creditLine, "7855521", "Se pagaron 1.45 toneladas de más")
            };

            foreach(AjusteBoletaDetalle ajusteDetalle in _ajusteBoletaDetalles)
            {
                ajusteDetalle.AjusteBoletaDetalleId++;
            }

            vendor.ProveedorId = 1;
        }

        //public Image ByteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    ms.Position = 0; // this is important
        //    Image image = Image.FromStream(ms);
        //    return image;
        //}

        [TestMethod]
        public void Create_AjusteBoleta()
        {
            //Arrange

            //Acti
            _ajusteBoletaDomainServices.TryCreateBoletaAjuste(_boleta, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();

            //Assert
            Assert.AreEqual(true, hasAjusteBoleta);
            Assert.AreEqual(string.Empty, errorMessage);
        }

        [TestMethod]
        public void Create_AjusteBoleta_no_puede_crear_sin_boleta()
        {
            //Arrange
            _boleta = null;

            //Acti
            _ajusteBoletaDomainServices.TryCreateBoletaAjuste(_boleta, out string errorMessage);

            //Assert
            Assert.AreEqual("BoletaId NO existe!", errorMessage);
        }

        [TestMethod]
        public void Create_AjusteBoleta_no_puede_crear_nuevamente_otro_ajuste_a_la_misma_boleta()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);

            //Acti
            _ajusteBoletaDomainServices.TryCreateBoletaAjuste(_boleta, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();

            //Assert
            Assert.AreEqual("La Boleta ya pertenece a un proceso de Ajustes", errorMessage);
            Assert.AreEqual(true, hasAjusteBoleta);
        }

        [TestMethod]
        public void DeleteAjusteBoleta()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            
            //Acti
            bool canDeleteAjusteBoleta = _ajusteBoletaDomainServices.TryDeleteAjusteBoleta(ajusteBoleta, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();

            //Assert
            Assert.AreEqual(true, canDeleteAjusteBoleta);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(false, hasAjusteBoleta);
        }

        [TestMethod]
        public void DeleteAjusteBoleta_no_puede_borrar_si_existe_un_abono_realizado()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);

            Boletas boletaPago = new Boletas("95874", "568745", _vendor, "Placa", "Motorista", 1, 1, 45, 30, 0m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePago = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 500);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePago);

            //Acti
            bool canDeleteAjusteBoleta = _ajusteBoletaDomainServices.TryDeleteAjusteBoleta(ajusteBoleta, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();

            //Assert
            Assert.AreEqual(false, canDeleteAjusteBoleta);
            Assert.AreEqual("Existen ajustes con abonos", errorMessage);
            Assert.AreEqual(true, hasAjusteBoleta);
        }

        [TestMethod]
        public void DeleteAjusteDetalle()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);

            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            
            //Acti
            bool canDeleteAjusteDetalle = _ajusteBoletaDomainServices.TryDeleteAjusteDetalle(ajusteDetalleKipper, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();
            int ajusteDetalleCount = ajusteBoleta.AjusteBoletaDetalles.Count;

            //Assert
            Assert.AreEqual(true, canDeleteAjusteDetalle);
            Assert.AreEqual(string.Empty, errorMessage);
            Assert.AreEqual(true, hasAjusteBoleta);
            Assert.AreEqual(2, ajusteDetalleCount);
        }

        [TestMethod]
        public void DeleteAjusteDetalle_no_puede_remover_cuando_ya_existe_un_abono_realizado()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);

            Boletas boletaPago = new Boletas("95874", "568745", _vendor, "Placa", "Motorista", 1, 1, 45, 30, 0m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePago = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 500);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePago);

            //Acti
            bool canDeleteAjusteDetalle = _ajusteBoletaDomainServices.TryDeleteAjusteDetalle(ajusteDetalleKipper, out string errorMessage);
            bool hasAjusteBoleta = _boleta.HasAjusteBoleta();
            int ajusteDetalleCount = ajusteBoleta.AjusteBoletaDetalles.Count;

            //Assert
            Assert.AreEqual(false, canDeleteAjusteDetalle);
            Assert.AreEqual("El Item del tipo Kipper del AjusteId 1 ya tiene pago", errorMessage);
            Assert.AreEqual(true, hasAjusteBoleta);
            Assert.AreEqual(3, ajusteDetalleCount);
        }

        [TestMethod]
        public void TryActivateAjusteBoleta()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));

            //Act
            bool canActivateAjuste = _ajusteBoletaDomainServices.TryActiveAjusteBoleta(ajusteBoleta, out string errorMessage);

            //Assert
            Assert.AreEqual(true, canActivateAjuste);
            Assert.AreEqual(Estados.ACTIVO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryActivateAjusteBoleta_no_puede_realizarlo_sin_detalle_de_ajustes()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);

            //Act
            bool canActivateAjuste = _ajusteBoletaDomainServices.TryActiveAjusteBoleta(ajusteBoleta, out string errorMessage);

            //Assert
            Assert.AreEqual(false, canActivateAjuste);
            Assert.AreEqual("El ajuste de boleta no tiene un detalle", errorMessage);
            Assert.AreEqual(Estados.NUEVO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryActivateAjusteBoleta_unicamente_activa_las_que_estan_en_estado_nuevo()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);

            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePayment);
            _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boletaPago, out string errorMessage);


            //Act
            bool canActivateAjuste = _ajusteBoletaDomainServices.TryActiveAjusteBoleta(ajusteBoleta,out errorMessage);

            //Assert
            Assert.AreEqual(false, canActivateAjuste);
            Assert.AreEqual("El estado del ajuste de boleta debe ser Nuevo", errorMessage);
            Assert.AreEqual(Estados.ENPROCESO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryApplyAjusteBoletaPayment()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);
            ajustePayment.AjusteBoletaPagoId = 1003;
            
            //Act
            bool canApplyPayment = _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boletaPago, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(true, canApplyPayment);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(250, ajusteBoletaAbonos);
            Assert.AreEqual(5001.83m, ajusteBoletaPendingAmount);
            Assert.AreEqual(Estados.ENPROCESO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void Evaluate_Status_Close()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaDetalle ajusteDetalleDeposito = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("deposito"));
            AjusteBoletaDetalle ajusteDetalleToneladas = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("toneladas"));

            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 1000);
            ajustePayment.AjusteBoletaPagoId = 1003;

            boletaPago.AddAjusteBoletaPago(ajustePayment);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePayment);

            AjusteBoletaPago ajustePayment2 = new AjusteBoletaPago(ajusteDetalleDeposito, boletaPago, 2500.85m);
            ajustePayment2.AjusteBoletaPagoId = 2;

            boletaPago.AddAjusteBoletaPago(ajustePayment2);
            ajusteDetalleDeposito.AddAjusteBoletaPago(ajustePayment2);

            AjusteBoletaPago ajustePayment3 = new AjusteBoletaPago(ajusteDetalleToneladas, boletaPago, 1750.98m);
            ajustePayment2.AjusteBoletaPagoId = 3;

            boletaPago.AddAjusteBoletaPago(ajustePayment3);
            ajusteDetalleToneladas.AddAjusteBoletaPago(ajustePayment3);

            boletaPago.Estado = Estados.CERRADO;

            //Act
            ajusteBoleta.UpdateStatus();
            string status = ajusteBoleta.Estado;

            //Assert
            Assert.AreEqual(status, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryApplyAjusteBoletaPayment_2_abonos()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaDetalle ajusteDetalleDeposito = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("deposito"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);
            ajustePayment.AjusteBoletaPagoId = 1;
            AjusteBoletaPago ajustePayment2 = new AjusteBoletaPago(ajusteDetalleDeposito, boletaPago, 500);
            ajustePayment2.AjusteBoletaPagoId = 2;

            boletaPago.AddAjusteBoletaPago(ajustePayment);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePayment);

            //Act
            bool canApplyPayment = _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment2, boletaPago, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(true, canApplyPayment);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(750, ajusteBoletaAbonos);
            Assert.AreEqual(4501.83m, ajusteBoletaPendingAmount);
            Assert.AreEqual(Estados.ENPROCESO, ajusteBoleta.Estado);
        }

        [TestMethod]
        public void TryApplyAjusteBoletaPayment_no_puede_pagar_si_la_boleta_no_es_activa()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);
            
            boletaPago.Estado = Estados.ENPROCESO;

            //Act
            bool canApplyPayment = _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boletaPago, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(false, canApplyPayment);
            Assert.AreEqual("La Boleta de Pago debe estar Activo", errorMessage);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(0, ajusteBoletaAbonos);
            Assert.AreEqual(5251.83m, ajusteBoletaPendingAmount);
        }

        [TestMethod]
        public void TryApplyAjusteBoletaPayment_no_puede_el_abono_supera_el_total_a_pagar_de_la_boleta()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 1000);

            BoletaOtrasDeducciones boletaDeduccion = new BoletaOtrasDeducciones(boletaPago.BoletaId, 13000, "test", "Manual", null, string.Empty, true);
            boletaPago.AgregarOtraDeduccion(boletaDeduccion);

            //Act
            bool canApplyPayment = _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boletaPago, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(false, canApplyPayment);
            Assert.AreEqual("El Abono del Ajuste supera el saldo de la boleta", errorMessage);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(0, ajusteBoletaAbonos);
            Assert.AreEqual(5251.83m, ajusteBoletaPendingAmount);
        }

        [TestMethod]
        public void TryApplyAjusteBoletaPayment_no_puede_el_abono_es_mayor_al_saldo_que_se_debe_cobrar()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 1001);
            
            //Act
            bool canApplyPayment = _ajusteBoletaDomainServices.TryApplyAjusteBoletaPayment(ajustePayment, boletaPago, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(false, canApplyPayment);
            Assert.AreEqual("El Abono supera el monto total del AjusteId #1", errorMessage);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(0, ajusteBoletaAbonos);
            Assert.AreEqual(5251.83m, ajusteBoletaPendingAmount);
        }

        [TestMethod]
        public void TryRemoveAjusteBoletaPayment()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaDetalle ajusteDetalleDeposito = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("deposito"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);
            ajustePayment.AjusteBoletaPagoId = 1;
            AjusteBoletaPago ajustePayment2 = new AjusteBoletaPago(ajusteDetalleDeposito, boletaPago, 500);
            ajustePayment2.AjusteBoletaPagoId = 2;

            boletaPago.AddAjusteBoletaPago(ajustePayment);
            boletaPago.AddAjusteBoletaPago(ajustePayment2);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePayment);
            ajusteDetalleDeposito.AddAjusteBoletaPago(ajustePayment2);

            List<AjusteBoletaPago> ajustePayments = new List<AjusteBoletaPago>();

            foreach (AjusteBoletaDetalle detail in ajusteBoleta.AjusteBoletaDetalles)
            {
                ajustePayments.AddRange(detail.AjusteBoletaPagos);
            }

            foreach (AjusteBoletaPago payment in ajustePayments)
            {
                payment.AjusteBoletaDetalle.AjusteBoleta = ajusteBoleta;
            }

            ajusteBoleta.UpdateStatus();

            //Act
            bool canDeletePayment = _ajusteBoletaDomainServices.TryRemoveAjusteBoletaPayment(boletaPago, ajustePayment2, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(true, canDeletePayment);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(250, ajusteBoletaAbonos);
            Assert.AreEqual(5001.83m, ajusteBoletaPendingAmount);
        }

        [TestMethod]
        public void TryRemoveAjusteBoletaPayment_no_puede_si_la_boleta_de_pago_no_Esta_activa()
        {
            //Arrange
            _boleta.AddAjusteBoleta(ajusteBoleta);
            ajusteBoleta.AjusteBoletaDetalles = new List<AjusteBoletaDetalle>(_ajusteBoletaDetalles);
            Boletas boletaPago = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, boletPath);
            boletaPago.BoletaId = 45;
            AjusteBoletaDetalle ajusteDetalleKipper = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("Kipper"));
            AjusteBoletaDetalle ajusteDetalleDeposito = _ajusteBoletaDetalles.FirstOrDefault(d => d.AjusteTipo.Descripcion.Contains("deposito"));
            AjusteBoletaPago ajustePayment = new AjusteBoletaPago(ajusteDetalleKipper, boletaPago, 250);
            ajustePayment.AjusteBoletaPagoId = 1;
            AjusteBoletaPago ajustePayment2 = new AjusteBoletaPago(ajusteDetalleDeposito, boletaPago, 500);
            ajustePayment2.AjusteBoletaPagoId = 2;

            boletaPago.AddAjusteBoletaPago(ajustePayment);
            boletaPago.AddAjusteBoletaPago(ajustePayment2);
            ajusteDetalleKipper.AddAjusteBoletaPago(ajustePayment);
            ajusteDetalleDeposito.AddAjusteBoletaPago(ajustePayment2);

            List<AjusteBoletaPago> ajustePayments = new List<AjusteBoletaPago>();

            foreach (AjusteBoletaDetalle detail in ajusteBoleta.AjusteBoletaDetalles)
            {
                ajustePayments.AddRange(detail.AjusteBoletaPagos);
            }

            ajusteBoleta.UpdateStatus();

            BoletaCierres boletacierre = new BoletaCierres(boletaPago.BoletaId, "Manual", 456, "345565", 120, DateTime.Now);
            boletaPago.AddBoletaCierre(boletacierre);
            boletaPago.ActualizarEstadoBoleta();

            //Act
            bool canDeletePayment = _ajusteBoletaDomainServices.TryRemoveAjusteBoletaPayment(boletaPago, ajustePayment2, out string errorMessage);
            decimal ajusteBoletaTotal = ajusteBoleta.GetAjusteBoletaTotal();
            decimal ajusteBoletaAbonos = ajusteBoleta.GetAjusteBoletaPayments();
            decimal ajusteBoletaPendingAmount = ajusteBoleta.GetAjusteBoletaPendingAmount();

            //Assert
            Assert.AreEqual(false, canDeletePayment);
            Assert.AreEqual(5251.83m, ajusteBoletaTotal);
            Assert.AreEqual(750, ajusteBoletaAbonos);
            Assert.AreEqual(4501.83m, ajusteBoletaPendingAmount);
            Assert.AreEqual(Estados.ENPROCESO, boletaPago.Estado);
        }
    }
}
