using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.ReglasNegocio.DomainServices;
using ComercialColindres.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Test.UnitTests.DomainServicesTests
{
    [TestClass]
    public class FacturaDetalleBoletaDomainServicesTests
    {
        readonly string BoletaPath = "";
        private IFacturaDetalleBoletaDomainServices _facturaDetalleBoletaDomainServices;
        Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        FacturasCategorias invoiceCategory = new FacturasCategorias
        {
            FacturaCategoriaId = 1,
            Descripcion = "Crédito"
        };
        ClientePlantas planta = new ClientePlantas("RTN", "Gildan", "5541666", "Choloma");
        SubPlanta subPlanta;
        Factura invoice;
        byte[] _tempImage;

        [TestInitialize]
        public void Inicializacion()
        {
            _facturaDetalleBoletaDomainServices = new FacturaDetalleBoletaDomainServices();
            planta.PlantaId = 1;
            planta.IsExempt = true;
            planta.HasExonerationNo = true;
            planta.HasSubPlanta = true;

            _tempImage = new byte[7];
            subPlanta = new SubPlanta(2, "Properties", "RTN", "choloma", true, "Registro")
            {
                Planta = planta,
                SubPlantaId = 1
            };

            invoice = new Factura(2, invoiceCategory, planta, subPlanta.SubPlantaId, "PO", string.Empty, "1258", "ProForma", DateTime.Now, 151785.85m,
                                  "ExonerationNo", 0m, true, 378596.26m, "test")
            {
                FacturaId = 1
            };

            MockSettingsFactory settingsFactory = new MockSettingsFactory();
            SettingFactory.SetCurrent(settingsFactory);
        }


        [TestMethod]
        public void TryToAssignBoletaToInvoice_scene_perfect_add()
        {
            //Arrange
            Boletas boleta = new Boletas("160548", "97895", vendor, "AAR 6396", "Darwin Archaga", 1, planta.PlantaId, 56.78m, 16.15m, 0m, 0, 1050, 1050, DateTime.Now, _tempImage, BoletaPath);
            boleta.BoletaId = 3456;
            FacturaDetalleBoletas facturaDetalleBoleta = new FacturaDetalleBoletas(invoice, null, "97895", string.Empty, 40.63m, DateTime.Now);
            facturaDetalleBoleta.FacturaDetalleBoletaId = 1;
            
            //Act
            bool canAssign = _facturaDetalleBoletaDomainServices.TryToAssignBoletaToInvoice(boleta, facturaDetalleBoleta, out string errorValidation);
            bool isAssigned = boleta.IsAssignedToInvoice();

            //Assert
            Assert.AreEqual(true, canAssign);
            Assert.AreEqual(string.Empty, errorValidation);
            Assert.AreEqual(false, isAssigned);
            Assert.AreEqual(boleta.BoletaId, facturaDetalleBoleta.BoletaId);
        }

        [TestMethod]
        public void TryToAssignBoletaToInvoice_scene_perfect_update()
        {
            //Arrange
            Boletas boleta = new Boletas("160548", "97895", vendor, "AAR 6396", "Darwin Archaga", 1, planta.PlantaId, 56.78m, 16.15m, 0m, 0, 1050, 1050, DateTime.Now, _tempImage, BoletaPath);
            boleta.BoletaId = 3456;
            FacturaDetalleBoletas facturaDetalleBoleta = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "97895", string.Empty, 40.63m, DateTime.Now);
            facturaDetalleBoleta.FacturaDetalleBoletaId = 1;
            boleta.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            boleta.FacturaDetalleBoletas.Add(facturaDetalleBoleta);

            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.AddInvoiceDetailBoleta(facturaDetalleBoleta);

            //Act
            bool canAssign = _facturaDetalleBoletaDomainServices.TryToAssignBoletaToInvoice(boleta, facturaDetalleBoleta, out string errorValidation);
            bool isAssigned = boleta.IsAssignedToInvoice();

            //Assert
            Assert.AreEqual(true, canAssign);
            Assert.AreEqual(string.Empty, errorValidation);
            Assert.AreEqual(true, isAssigned);
            Assert.AreEqual(boleta.BoletaId, facturaDetalleBoleta.BoletaId);
        }

        [TestMethod]
        public void TryToAssignBoletaToInvoice_la_boleta_solo_puede_estar_asignada_a_una_factura_add()
        {
            //Arrange
            Boletas boletaOld = new Boletas("160549", "97895", vendor, "AAR 6396", "Darwin Archaga", 1, planta.PlantaId, 56.78m, 16.15m, 0m, 0, 1050, 1050, DateTime.Now, _tempImage, BoletaPath);
            boletaOld.BoletaId = 3325;

            Boletas boletaNew = new Boletas("160548", "97895", vendor, "AAR 6396", "Darwin Archaga", 1, planta.PlantaId, 56.78m, 16.15m, 0m, 0, 1050, 1050, DateTime.Now, _tempImage, BoletaPath);
            boletaNew.BoletaId = 3456;
            FacturaDetalleBoletas facturaDetalleBoleta = new FacturaDetalleBoletas(invoice, boletaOld.BoletaId, "97895", string.Empty, 40.63m, DateTime.Now);
            facturaDetalleBoleta.FacturaDetalleBoletaId = 1;
            boletaOld.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            boletaOld.FacturaDetalleBoletas.Add(facturaDetalleBoleta);

            //Act
            bool canAssign = _facturaDetalleBoletaDomainServices.TryToAssignBoletaToInvoice(boletaNew, facturaDetalleBoleta, out string errorValidation);
            bool isAssigned = boletaNew.IsAssignedToInvoice();
            bool isAssignedOld = boletaOld.IsAssignedToInvoice();

            //Assert
            Assert.AreEqual(false, canAssign);
            Assert.AreEqual("Existe una Boleta con el mismo #Envío asignada a una Factura", errorValidation);
            Assert.AreEqual(false, isAssigned);
            Assert.AreEqual(true, isAssignedOld);
        }
    }
}
