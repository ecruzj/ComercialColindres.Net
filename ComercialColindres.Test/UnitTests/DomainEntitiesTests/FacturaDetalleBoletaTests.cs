using ComercialColindres.Datos.Entorno.DataCore.Setting;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Test.UnitTests.DomainEntitiesTests
{
    [TestClass]
    public class FacturaDetalleBoletaTests
    {
        readonly string BoletaPath = "";

        FacturasCategorias invoiceCategory = new FacturasCategorias
        {
            Descripcion = "Credito",
            FacturaCategoriaId = 1
        };
        ClientePlantas factory = new ClientePlantas("RTN", "Gildan", "5554", "Choloma");
        Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        Boletas boleta;
        Factura invoice;
        CategoriaProductos raquis;
        CategoriaProductos mesocarpio;
        CategoriaProductos taco;
        CategoriaProductos cascaraCafe;
        CategoriaProductos casullaCafe;
        CategoriaProductos aserrin;
        CategoriaProductos maderaEnRollo;
        CategoriaProductos piller;
        byte[] _tempImage;

        [TestInitialize]
        public void Inicializacion()
        {
            _tempImage = new byte[7];
            vendor.ProveedorId = 1;
            factory.RequiresPurchaseOrder = true;
            factory.HasSubPlanta = false;
            factory.IsExempt = true;
            factory.HasExonerationNo = true;
            factory.PlantaId = 2;

            boleta = new Boletas("120875", "58795", vendor, "Placa", "Motorista", 6, factory.PlantaId, 52.49m, 14.25m, 0m, 0, 1045m, 1045, DateTime.Now, _tempImage, BoletaPath)
            {
                BoletaId = 8765
            };

            invoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "PF2019-58", DateTime.Now, 153798.60m, "ExonerationXXX", 0, true, 3786521.53m, "Semana 20")
            {
                FacturaId = 9873
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

            aserrin = new CategoriaProductos(1, "Aserrin Nuevo")
            {
                CategoriaProductoId = 6
            };

            maderaEnRollo = new CategoriaProductos(1, "Madera en Rollo")
            {
                CategoriaProductoId = 7
            };

            piller = new CategoriaProductos(1, "Piller")
            {
                CategoriaProductoId = 8
            };

            MockSettingsFactory settingsFactory = new MockSettingsFactory();
            SettingFactory.SetCurrent(settingsFactory);
        }

        [TestMethod]
        public void GetSalePrice_debe_tener_boleta_asignada_y_existir_la_categoria_de_producto_en_el_detalle_item_y_es_moneda_extranjera()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(1107.90m, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_devuelve_el_valor_de_compra_de_la_boleta_con_precio_unitario_con_moneda_foranea()
        {
            //Arange
            invoice.HasUnitPriceItem = true;

            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now, 40m)
            {
                Boleta = boleta
            };
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>
            {
                boletaDetail
            };

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(984.80m, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_devuelve_el_valor_de_compra_de_la_boleta_con_precio_unitario_factura_es_moneda_local()
        {
            //Arange
            Factura newInvoice = new Factura(1, invoiceCategory, factory, null, "223323", "WK28", "4356", "", DateTime.Now, 143859m, "", 0, false, 0m, "Semana 20", true)
            {
                FacturaId = 9874
            };

            newInvoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 83.46m, aserrin, 850m),
                CreateInvoceDetailItem(invoice, 81.02m, aserrin, 900m)
            };

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(newInvoice, boleta.BoletaId, "N/A", "120875", 45.04m, DateTime.Now, 850m)
            {
                Boleta = boleta
            };
            newInvoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>
            {
                boletaDetail
            };

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(850, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_debe_tener_boleta_asignada_y_existir_la_categoria_de_producto_en_el_detalle_item_y_es_moneda_local()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            invoice.IsForeignCurrency = false;
            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(45m, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_no_hay_boleta_asignada()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(0m, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_tiene_boleta_pero_no_hay_categoria_producto_en_detalle_item()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m)
            };

            boleta.CategoriaProducto = aserrin;
            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(0m, salePrice);
        }

        [TestMethod]
        public void GetSalePrice_tiene_boleta_y_es_un_derivado_del_aserrin()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 1450m, aserrin, 45m)
            };

            boleta.CategoriaProducto = piller;
            boleta.CategoriaProductoId = 8;

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);


            //Act
            decimal salePrice = boletaDetail.GetSalePrice();

            //Assert
            Assert.AreEqual(1107.90m, salePrice);
        }

        [TestMethod]
        public void GetProfit_debe_tener_boleta_asignada_y_existir_la_categoria_de_producto_en_el_detalle_item_y_es_moneda_extranjera()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal profit = boletaDetail.GetProfit();

            //Assert
            Assert.AreEqual(2405.30m, profit);
        }

        [TestMethod]
        public void GetProfit_con_penalidad_en_la_compra()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 249.62m, raquis, 37),
                CreateInvoceDetailItem(invoice, 287.76m, mesocarpio, 41),
                CreateInvoceDetailItem(invoice, 406.26m, taco, 41.50m),
                CreateInvoceDetailItem(invoice, 279.96m, casullaCafe, 44.50m),
                CreateInvoceDetailItem(invoice, 74.42m, casullaCafe, 45m),
                CreateInvoceDetailItem(invoice, 2178.85m, aserrin, 45m),
            };

            boleta.CategoriaProductoId = taco.CategoriaProductoId;
            boleta.PrecioProductoCompra = 860m;
            boleta.CantidadPenalizada = 2.30m;
            boleta.PesoProducto = 39.56m;
            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 41.86m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal profit = boletaDetail.GetProfit();

            //Assert
            Assert.AreEqual(8748.02m, profit);
        }

        [TestMethod]
        public void GetProfit_debe_tener_boleta_asignada_y_existir_la_categoria_de_producto_en_el_detalle_item_y_es_moneda_local()
        {
            //Arange
            invoice.FacturaDetalleItems = new HashSet<FacturaDetalleItem>()
            {
                CreateInvoceDetailItem(invoice, 985.60m, aserrin, 900m),
            };

            invoice.IsForeignCurrency = false;
            FacturaDetalleBoletas boletaDetail = new FacturaDetalleBoletas(invoice, boleta.BoletaId, "58795", "120875", 38.24m, DateTime.Now);
            boletaDetail.Boleta = boleta;
            boleta.PrecioProductoCompra = 850m;
            invoice.FacturaDetalleBoletas = new HashSet<FacturaDetalleBoletas>();
            invoice.FacturaDetalleBoletas.Add(boletaDetail);

            //Act
            decimal profit = boletaDetail.GetProfit();

            //Assert
            Assert.AreEqual(1912m, profit);
        }

        private FacturaDetalleItem CreateInvoceDetailItem(Factura invoice, decimal cantidad, CategoriaProductos categoriaProducto, decimal precio)
        {
            return new FacturaDetalleItem(invoice, cantidad, categoriaProducto, precio);
        }
    }
}
