using ComercialColindres.Datos.Entorno.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Test.UnitTests.DomainEntitiesTests
{
    [TestClass]
    public class FacturaTests
    {
        [TestMethod]
        public void CreateInvoice_escenario_correcto()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.HasSubPlanta = false;
            factory.IsExempt = true;
            factory.HasExonerationNo = true;
            factory.PlantaId = 2;

            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 153798.60m, "ExonerationXXX", 0, true, 3786521.53m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(0, validationErrors.Count());
        }

        [TestMethod]
        public void CreateInvoice_no_puede_crear_factura_si_planta_es_exenta_y_pide_exoneracionNo()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.HasSubPlanta = true;
            factory.IsExempt = true;
            factory.HasExonerationNo = true;
            factory.PlantaId = 2;

            SubPlanta subPlanta = new SubPlanta(factory.PlantaId, "Properties S de RL", "05051978", "Choloma", true, "R2018554");
            subPlanta.SubPlantaId = 1;

            Factura invoice = new Factura(1, invoiceCategory, factory, subPlanta.SubPlantaId, "223323", "WK28", "4356", "PF19-0335", DateTime.Now, 153798.60m, string.Empty, 0, true, 3786521.53m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(1, validationErrors.Count());
            Assert.AreEqual("ExonerationNo es requerido", validationErrors.FirstOrDefault());
        }

        [TestMethod]
        public void CreateInvoice_no_puede_crear_factura_sin_proforma_cuando_esta_lo_requiere()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma")
            {
                RequiresPurchaseOrder = true,
                RequiresProForm = true,
                HasSubPlanta = true,
                IsExempt = true,
                PlantaId = 2
            };

            SubPlanta subPlanta = new SubPlanta(factory.PlantaId, "Properties S de RL", "05051978", "Choloma", true, "R2018554")
            {
                SubPlantaId = 1
            };

            Factura invoice = new Factura(1, invoiceCategory, factory, subPlanta.SubPlantaId, "223323", "WK28", "4356", string.Empty, DateTime.Now, 153798.60m, "ExonerationXXX", 0, true, 3786521.53m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(1, validationErrors.Count());
            Assert.AreEqual("ProFormaNo es requerido", validationErrors.FirstOrDefault());
        }

        [TestMethod]
        public void CreateInvoice_no_puede_crear_factura_sin_subPlanta_cuando_esta_lo_requiere()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.HasSubPlanta = true;
            factory.IsExempt = true;
            factory.PlantaId = 2;

            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 153798.60m, "ExonerationXXX", 0, true, 3786521.53m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(1, validationErrors.Count());
            Assert.AreEqual("SubPlantaId es requerido", validationErrors.FirstOrDefault());
        }

        //aqui
        [TestMethod]
        public void CreateInvoice_no_puede_crear_sin_exoneracion_cuando_es_exenta_y_planta_requiere_exonerationNo()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = true;
            factory.HasExonerationNo = true;
            factory.HasSubPlanta = true;
            factory.PlantaId = 2;

            SubPlanta subPlanta = new SubPlanta(factory.PlantaId, "Properties S de RL", "05051978", "Choloma", true, "R2018554");
            subPlanta.SubPlantaId = 1;

            Factura invoice = new Factura(1, invoiceCategory, factory, subPlanta.SubPlantaId, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 153798.60m, string.Empty, 0, true, 3786521.53m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(1, validationErrors.Count());
            Assert.AreEqual("ExonerationNo es requerido", validationErrors.FirstOrDefault());
        }

        [TestMethod]
        public void CreateInvoice_no_puede_crear_sin_moneda_local_cuando_es_menada_extranjera()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = true;
            factory.PlantaId = 2;

            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 153798.60m, "ExonerationXXX", 0, true, 0m, "Semana 20");

            //Act
            IEnumerable<string> validationErrors = invoice.GetValidationErrors();

            //Assert
            Assert.AreEqual(1, validationErrors.Count());
            Assert.AreEqual("LocalCurrencyAmount es requerido", validationErrors.FirstOrDefault());
        }

        [TestMethod]
        public void CreateInvoice_calculos_para_foreign_currency_invoice_y_es_exenta()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = true;
            factory.PlantaId = 2;

            CategoriaProductos raquis = new CategoriaProductos(1, "Raquis");
            raquis.CategoriaProductoId = 1;
            CategoriaProductos mesocarpio = new CategoriaProductos(1, "Mesocarpio");
            mesocarpio.CategoriaProductoId = 2;
            CategoriaProductos taco = new CategoriaProductos(1, "Taco con Aserrin");
            taco.CategoriaProductoId = 3;
            CategoriaProductos cascaraCafe = new CategoriaProductos(1, "Cascara de Café");
            cascaraCafe.CategoriaProductoId = 4;
            CategoriaProductos casullaCafe = new CategoriaProductos(1, "Casulla de Café");
            casullaCafe.CategoriaProductoId = 5;
            CategoriaProductos aserrin = new CategoriaProductos(1, "Aserrin Nuevo");
            aserrin.CategoriaProductoId = 6;


            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 151749.26m, "ExonerationXXX", 0, true, 3710250.87m, "Semana 20");
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            //Act
            decimal subTotalInvoiceDollar = invoice.GetInvoiceSubTotalDollar();
            decimal subTotalInvoiceLps = invoice.GetInvoiceSubTotalLps();
            decimal totalInvoice = invoice.GetInvoiceForeignCurrencyTotal();
            decimal invoiceTax = invoice.GetInvoiceTax();

            //Assert
            Assert.AreEqual(151749.26m, totalInvoice);
            Assert.AreEqual(151749.26m, subTotalInvoiceDollar);
            Assert.AreEqual(3710250.87m, subTotalInvoiceLps);
            Assert.AreEqual(0m, invoiceTax);
        }

        [TestMethod]
        public void CreateInvoice_calculos_para_foreign_currency_invoice_y_es_exenta_y_tiene_nota_credito()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = true;
            factory.PlantaId = 2;

            CategoriaProductos raquis = new CategoriaProductos(1, "Raquis");
            raquis.CategoriaProductoId = 1;
            CategoriaProductos mesocarpio = new CategoriaProductos(1, "Mesocarpio");
            mesocarpio.CategoriaProductoId = 2;
            CategoriaProductos taco = new CategoriaProductos(1, "Taco con Aserrin");
            taco.CategoriaProductoId = 3;
            CategoriaProductos cascaraCafe = new CategoriaProductos(1, "Cascara de Café");
            cascaraCafe.CategoriaProductoId = 4;
            CategoriaProductos casullaCafe = new CategoriaProductos(1, "Casulla de Café");
            casullaCafe.CategoriaProductoId = 5;
            CategoriaProductos aserrin = new CategoriaProductos(1, "Aserrin Nuevo");
            aserrin.CategoriaProductoId = 6;


            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 151749.26m, "ExonerationXXX", 0, true, 3710250.87m, "Semana 20");
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            invoice.NotasCredito = new List<NotaCredito>()
            {
                new NotaCredito(invoice, "5378", 250, "Humedad")
            };

            //Act
            decimal subTotalInvoiceDollar = invoice.GetInvoiceSubTotalDollar();
            decimal subTotalInvoiceLps = invoice.GetInvoiceSubTotalLps();
            decimal totalInvoice = invoice.GetInvoiceForeignCurrencyTotal();
            decimal invoiceTax = invoice.GetInvoiceTax();

            //Assert
            Assert.AreEqual(151499.26m, totalInvoice);
            Assert.AreEqual(151499.26m, subTotalInvoiceDollar);
            Assert.AreEqual(3704138.40m, subTotalInvoiceLps);
            Assert.AreEqual(0m, invoiceTax);
        }

        [TestMethod]
        public void CreateInvoice_calculos_para_foreign_currency_invoice_y_no_es_exenta()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = false;
            factory.PlantaId = 2;

            CategoriaProductos raquis = new CategoriaProductos(1, "Raquis");
            raquis.CategoriaProductoId = 1;
            CategoriaProductos mesocarpio = new CategoriaProductos(1, "Mesocarpio");
            mesocarpio.CategoriaProductoId = 2;
            CategoriaProductos taco = new CategoriaProductos(1, "Taco con Aserrin");
            taco.CategoriaProductoId = 3;
            CategoriaProductos cascaraCafe = new CategoriaProductos(1, "Cascara de Café");
            cascaraCafe.CategoriaProductoId = 4;
            CategoriaProductos casullaCafe = new CategoriaProductos(1, "Casulla de Café");
            casullaCafe.CategoriaProductoId = 5;
            CategoriaProductos aserrin = new CategoriaProductos(1, "Aserrin Nuevo");
            aserrin.CategoriaProductoId = 6;


            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 151749.26m, string.Empty, 15, true, 3710250.87m, "Semana 20");
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            //Act
            decimal subTotalInvoiceDollar = invoice.GetInvoiceSubTotalDollar();
            decimal subTotalInvoiceLps = invoice.GetInvoiceSubTotalLps();
            decimal totalInvoice = invoice.GetInvoiceForeignCurrencyTotal();
            decimal invoiceTax = invoice.GetInvoiceTax();

            //Assert
            Assert.AreEqual(151749.26m, totalInvoice);
            Assert.AreEqual(151749.26m, subTotalInvoiceDollar);
            Assert.AreEqual(3710250.87m, subTotalInvoiceLps);
            Assert.AreEqual(0m, invoiceTax);
        }

        [TestMethod]
        public void CreateInvoice_calculos_para_local_currency_invoice_y_no_es_exenta()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = false;
            factory.PlantaId = 2;

            CategoriaProductos raquis = new CategoriaProductos(1, "Raquis");
            raquis.CategoriaProductoId = 1;
            CategoriaProductos mesocarpio = new CategoriaProductos(1, "Mesocarpio");
            mesocarpio.CategoriaProductoId = 2;
            CategoriaProductos taco = new CategoriaProductos(1, "Taco con Aserrin");
            taco.CategoriaProductoId = 3;
            CategoriaProductos cascaraCafe = new CategoriaProductos(1, "Cascara de Café");
            cascaraCafe.CategoriaProductoId = 4;
            CategoriaProductos casullaCafe = new CategoriaProductos(1, "Casulla de Café");
            casullaCafe.CategoriaProductoId = 5;
            CategoriaProductos aserrin = new CategoriaProductos(1, "Aserrin Nuevo");
            aserrin.CategoriaProductoId = 6;


            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 151749.26m, string.Empty, 15, false, 0, "Semana 20");
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            //Act
            decimal subTotalInvoiceDollar = invoice.GetInvoiceSubTotalDollar();
            decimal subTotalInvoiceLps = invoice.GetInvoiceSubTotalLps();
            decimal invoiceTax = invoice.GetInvoiceTax();
            decimal totalInvoice = invoice.GetInvoiceLocalCurrencyTotal();

            //Assert
            Assert.AreEqual(174511.65m, totalInvoice);
            Assert.AreEqual(0m, subTotalInvoiceDollar);
            Assert.AreEqual(151749.26m, subTotalInvoiceLps);
            Assert.AreEqual(22762.39m, invoiceTax);
        }

        [TestMethod]
        public void CreateInvoice_calculos_para_local_currency_invoice_y_es_exenta()
        {
            //Arrange
            FacturasCategorias invoiceCategory = new FacturasCategorias
            {
                Descripcion = "Credito",
                FacturaCategoriaId = 1
            };

            ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
            factory.RequiresPurchaseOrder = true;
            factory.IsExempt = true;
            factory.PlantaId = 2;

            CategoriaProductos raquis = new CategoriaProductos(1, "Raquis");
            raquis.CategoriaProductoId = 1;
            CategoriaProductos mesocarpio = new CategoriaProductos(1, "Mesocarpio");
            mesocarpio.CategoriaProductoId = 2;
            CategoriaProductos taco = new CategoriaProductos(1, "Taco con Aserrin");
            taco.CategoriaProductoId = 3;
            CategoriaProductos cascaraCafe = new CategoriaProductos(1, "Cascara de Café");
            cascaraCafe.CategoriaProductoId = 4;
            CategoriaProductos casullaCafe = new CategoriaProductos(1, "Casulla de Café");
            casullaCafe.CategoriaProductoId = 5;
            CategoriaProductos aserrin = new CategoriaProductos(1, "Aserrin Nuevo");
            aserrin.CategoriaProductoId = 6;
            
            Factura invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 151749.26m, "Exoneration3333", 0, false, 0, "Semana 20");
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            //Act
            decimal subTotalInvoiceDollar = invoice.GetInvoiceSubTotalDollar();
            decimal subTotalInvoiceLps = invoice.GetInvoiceSubTotalLps();
            decimal invoiceTax = invoice.GetInvoiceTax();
            decimal totalInvoice = invoice.GetInvoiceLocalCurrencyTotal();

            //Assert
            Assert.AreEqual(151749.26m, totalInvoice);
            Assert.AreEqual(0m, subTotalInvoiceDollar);
            Assert.AreEqual(151749.26m, subTotalInvoiceLps);
            Assert.AreEqual(0, invoiceTax);
        }

        private FacturaDetalleItem CreateInvoceDetailItem(Factura invoice, decimal cantidad, CategoriaProductos categoriaProducto, decimal precio)
        {
            return new FacturaDetalleItem(invoice, cantidad, categoriaProducto, precio);
        }
    }
}
