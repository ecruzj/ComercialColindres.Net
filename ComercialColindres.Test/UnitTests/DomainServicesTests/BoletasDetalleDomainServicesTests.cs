using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.ReglasNegocio.DomainServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Test.UnitTests.DomainServicesTests
{
    [TestClass]
    public class BoletasDetalleDomainServicesTests
    {
        readonly string BoletaPath = "";
        private IBoletasDetalleDomainServices _boletasDetalleDomainServices;
        Proveedores vendor = new Proveedores("0501-1992-008820", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
        byte[] _tempImage;

        [TestInitialize]
        public void Inicializacion()
        {
            _boletasDetalleDomainServices = new BoletasDetalleDomainServices();
            vendor.ProveedorId = 1;
            _tempImage = new byte[7];
        }
        
        [TestMethod]
        public void AgregarBoletaDetalle_debe_ingresar_el_tipo_de_deduccion_correcto()
        {
            //Arange
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, BoletaPath);
            boleta.BoletaId = 3456;
            var tipoDeduccion = "Tasa Seguridad";

            var deducciones = new HashSet<Deducciones>()
            {
                new Deducciones { DeduccionId = 1, Descripcion = "Prestamo Efectivo" },
                new Deducciones { DeduccionId = 2, Descripcion = "Orden Combustible" },
                new Deducciones { DeduccionId = 2, Descripcion = "Descarga de Producto" },
                new Deducciones { DeduccionId = 2, Descripcion = "Tasa Seguridad" },
                new Deducciones { DeduccionId = 2, Descripcion = "Abono por Boleta" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otras Deducciones" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otros Ingresos" }
            };

            //Acti
            var mensajeValidacion = _boletasDetalleDomainServices.AgregarBoletaDetalle(boleta, deducciones.ToList(), 30, tipoDeduccion, "N/A", "");

            //Asert
            Assert.AreEqual("", mensajeValidacion);
        }

        [TestMethod]
        public void ObtenerBoletaDeducciones_debe_armar_todas_las_posibles_deducciones_de_una_boleta_con_9_deducciones_y_2_abonos_positivos()
        {
            //Arange
            var proveedor = new Proveedores("RTN", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, BoletaPath);
            boleta.Proveedor = proveedor;
            var cuadrilla = new Cuadrillas("Colocho", 1, true);

            cuadrilla.CuadrillaId = 1;
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.Cuadrilla = cuadrilla;

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };
            
            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            prestamo1.PrestamoId = 1;
            prestamo2.PrestamoId = 2;

            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 6500, boleta.CodigoBoleta, "Segundo Abono")
            };

            abonosPrestamos.FirstOrDefault(p => p.PrestamoId == 1).Prestamo = prestamo1;
            abonosPrestamos.FirstOrDefault(p => p.PrestamoId == 2).Prestamo = prestamo2;

            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>();

            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, 12500, "Prestamos de Llantas", "Interbanca", 234, "TestRef1"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, 15600, "Prestamo de Mantenimiento Motor", "Interbanca", 234, "TestRef2"));
            

            var deducciones = new HashSet<Deducciones>()
            {
                new Deducciones { DeduccionId = 1, Descripcion = "Prestamo Efectivo" },
                new Deducciones { DeduccionId = 2, Descripcion = "Orden Combustible" },
                new Deducciones { DeduccionId = 2, Descripcion = "Descarga de Producto" },
                new Deducciones { DeduccionId = 2, Descripcion = "Tasa Seguridad" },
                new Deducciones { DeduccionId = 2, Descripcion = "Abono por Boleta" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otras Deducciones" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otros Ingresos" }
            };

            //Acti
            var boletaDeducciones = _boletasDetalleDomainServices.ObtenerBoletaDeducciones(boleta, deducciones.ToList());
            var totalFactura = boleta.ObtenerTotalFacturaCompra();
            var totalDeducciones = boleta.ObtenerTotalDeduccion();
            var totalAPagar = boleta.ObtenerTotalAPagar();
            var tasaSeguridad = boleta.ObtenerTasaSeguridad();

            //Asert
            Assert.AreEqual(10, boletaDeducciones.Count);
            Assert.AreEqual(13794, totalFactura);
            Assert.AreEqual(35370, totalDeducciones);
            Assert.AreEqual(6524, totalAPagar);
            Assert.AreEqual(30, tasaSeguridad);
        }

        [TestMethod]
        public void ObtenerBoletaDeducciones_debe_armar_todas_las_posibles_deducciones_de_una_boleta_con_9_deducciones_y_2_abonos_positivos_y_TS_solo_10_centavos()
        {
            //Arange
            var proveedor = new Proveedores("RTN", "Josue Cruz", "0501-1992-00882", "SPS", "9445-8425", "ecruzj@outlook.com");
            proveedor.IsExempt = true;
            var boleta = new Boletas("Test", "TestEnvio", vendor, "Placa", "Motorista", 1, 1, 45, 30, 0.48m, 0, 950, 1100, DateTime.Now, _tempImage, BoletaPath);
            boleta.Proveedor = proveedor;
            var cuadrilla = new Cuadrillas("Colocho", 1, true);

            cuadrilla.CuadrillaId = 1;
            boleta.Descargador = new Descargadores(boleta.BoletaId, 1, 990, DateTime.Now, false);
            boleta.Descargador.Cuadrilla = cuadrilla;

            var ordenesCombustible = new HashSet<OrdenesCombustible>
            {
                new OrdenesCombustible("Fact1", 1, "Josue Cruz", "Placa", false, 8900, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId),
                new OrdenesCombustible("Fact2", 1, "Josue Cruz", "Placa", false, 9500, "test Orden", DateTime.Now, _tempImage, vendor.ProveedorId)
            };

            boleta.OrdenesCombustible = new HashSet<OrdenesCombustible>(ordenesCombustible);

            var prestamo1 = new Prestamos("Prestamo1", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 42000, "Testing");
            var prestamo2 = new Prestamos("Prestamo2", 1, 1, DateTime.Now, "Josue Cruz", 0.10m, 58000, "Testing");
            prestamo1.PrestamoId = 1;
            prestamo2.PrestamoId = 2;

            var abonosPrestamos = new HashSet<PagoPrestamos>
            {
                new PagoPrestamos(prestamo1.PrestamoId, "Manual", null, boleta.BoletaId, 5000, boleta.CodigoBoleta, "Primer Abono"),
                new PagoPrestamos(prestamo2.PrestamoId, "Manual", null, boleta.BoletaId, 6500, boleta.CodigoBoleta, "Segundo Abono")
            };

            abonosPrestamos.FirstOrDefault(p => p.PrestamoId == 1).Prestamo = prestamo1;
            abonosPrestamos.FirstOrDefault(p => p.PrestamoId == 2).Prestamo = prestamo2;

            boleta.PagoPrestamos = new HashSet<PagoPrestamos>(abonosPrestamos);

            boleta.BoletaOtrasDeducciones = new HashSet<BoletaOtrasDeducciones>();

            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, -2600, "Pago Flete a Wilmer Colindres", "Interbanca", 234, "TestRef1"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, -1850, "Pago Flete a Rodolfo Cruz", "Interbanca", 234, "TestRef2"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, 12500, "Prestamos de Llantas", "Interbanca", 234, "TestRef1"));
            boleta.BoletaOtrasDeducciones.Add(new BoletaOtrasDeducciones(boleta.BoletaId, 15600, "Prestamo de Mantenimiento Motor", "Interbanca", 234, "TestRef2"));


            var deducciones = new HashSet<Deducciones>()
            {
                new Deducciones { DeduccionId = 1, Descripcion = "Prestamo Efectivo" },
                new Deducciones { DeduccionId = 2, Descripcion = "Orden Combustible" },
                new Deducciones { DeduccionId = 2, Descripcion = "Descarga de Producto" },
                new Deducciones { DeduccionId = 2, Descripcion = "Tasa Seguridad" },
                new Deducciones { DeduccionId = 2, Descripcion = "Abono por Boleta" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otras Deducciones" },
                new Deducciones { DeduccionId = 2, Descripcion = "Boleta Otros Ingresos" }
            };

            //Acti
            var boletaDeducciones = _boletasDetalleDomainServices.ObtenerBoletaDeducciones(boleta, deducciones.ToList());
            var totalFactura = boleta.ObtenerTotalFacturaCompra();
            var totalDeducciones = boleta.ObtenerTotalDeduccion();
            var totalAPagar = boleta.ObtenerTotalAPagar();
            var tasaSeguridad = boleta.ObtenerTasaSeguridad();

            //Asert
            Assert.AreEqual(10, boletaDeducciones.Count);
            Assert.AreEqual(13794, totalFactura);
            Assert.AreEqual(35340.10m, totalDeducciones);
            Assert.AreEqual(6553.90m, totalAPagar);
            Assert.AreEqual(0.10m, tasaSeguridad);
        }
    }
}
