using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ComercialColindres.Test.UnitTests.DomainEntitiesTests
{
    [TestClass]
    public class BoletaHumedadTests
    {
        readonly string boletPath = "";
        readonly Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        ClientePlantas planta = new ClientePlantas("RTN", "Caracol Knits", "25550", "Poterillos, Cortes");
        Boletas boletaPayment;
        Boletas boletaWithHumidity1;
        byte[] _tempImage;

        [TestInitialize]
        public void Inicializacion()
        {
            _tempImage = new byte[7];
            planta.PlantaId = 2;
            vendor.ProveedorId = 10;
            boletaPayment = new Boletas("118185", "59802", vendor, "aar 2028", "eden aguilar", 5, 2, 62.50m, 17.85m, 44.65m, 0, 850, 1000, DateTime.Now, _tempImage, boletPath);
            boletaWithHumidity1 = new Boletas("Test", "59802", vendor, "Placa", "Motorista", 1, 1, 58.78m, 17.20m, 0m, 0, 730, 850, DateTime.Now, _tempImage, boletPath);

            MockSettingsFactory settingsFactory = new MockSettingsFactory();
            SettingFactory.SetCurrent(settingsFactory);
        }

        [TestMethod]
        public void GetWrongPercentageOfHumidity()
        {
            //Arrange
            BoletaHumedad boletaHumedad = new BoletaHumedad("59802", planta, false, 58.75m, 55, DateTime.Now);

            //Act
            decimal percent = boletaHumedad.GetWrongPercentageOfHumidity();

            //Assert
            Assert.AreEqual(3.75m, percent);
        }

        [TestMethod]
        public void CalculateHumidityPricePayment_when_grown_percent_is_bigger_than_0()
        {
            //Arrange
            BoletaHumedad boletaHumedad = new BoletaHumedad("59802", planta, false, 59.21m, 55, DateTime.Now);
            boletaHumedad.BoletaHumedadId = 4534;

            BoletaHumedadAsignacion boletaHumedadAsignacion = new BoletaHumedadAsignacion(boletaWithHumidity1, boletaHumedad);
            boletaHumedadAsignacion.BoletaHumedadAsignacionId = 7845;
            boletaHumedad.BoletaHumedadAsignacion = boletaHumedadAsignacion;
            
            //Act
            decimal humidityPricePayment = boletaHumedad.CalculateHumidityPricePayment();

            //Assert
            Assert.AreEqual(1277.5m, humidityPricePayment);
        }
    }
}
