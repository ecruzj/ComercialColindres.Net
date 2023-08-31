using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ComercialColindres.Test.UnitTests.DomainEntitiesTests
{
    [TestClass]
    public class PrestamosTests
    {
        [TestMethod]
        public void ObtenerTotalACobrar_interes_mensual()
        {
            //Arrange
            DateTime fechaPrestamo = Convert.ToDateTime("12/12/2019");

            Prestamos miPrestamo = new Prestamos("PR20-00010", 1, 1, fechaPrestamo, "Josue Cruz", 10, 120000, "Prestamo con interes mensual", true);

            //Acti
            decimal totalCobrar = miPrestamo.ObtenerTotalACobrar();

            ///parte manual por el cambio de fechas
            decimal interesMensual = Math.Round((miPrestamo.MontoPrestamo * (miPrestamo.PorcentajeInteres / 100)), 2);
            decimal interesDiario = Math.Round((interesMensual / 30), 2);
            int diasTranscurridos = (DateTime.Now.Date - miPrestamo.FechaCreacion.Date).Days;

            decimal totalPendiente = Math.Round((diasTranscurridos * interesDiario) + miPrestamo.MontoPrestamo, 2);

            //Asset
            Assert.AreEqual(totalPendiente, totalCobrar);
        }

        [TestMethod]
        public void ObtenerTotalACobrar_interes_unico()
        {
            //Arrange
            DateTime fechaPrestamo = Convert.ToDateTime("12/12/2019");

            Prestamos miPrestamo = new Prestamos("PR20-00010", 1, 1, fechaPrestamo, "Josue Cruz", 10, 120000, "Prestamo con interes mensual");

            //Acti
            decimal totalCobrar = miPrestamo.ObtenerTotalACobrar();

            //Asset
            Assert.AreEqual(132000, totalCobrar);
        }

        [TestMethod]
        public void ObtenerTotalACobrar_interes_mensual_ya_pagado()
        {
            //Arrange
            DateTime fechaPrestamo = Convert.ToDateTime("12/12/2019");

            Prestamos miPrestamo = new Prestamos("PR20-00010", 1, 1, fechaPrestamo, "Josue Cruz", 10, 120000, "Prestamo con interes mensual", true)
            {
                PrestamoId = 1
            };

            DateTime fechaPago = Convert.ToDateTime("01/31/2020");
            PagoPrestamos payment = new PagoPrestamos(miPrestamo.PrestamoId, "Manual", null, null, 140000, string.Empty, string.Empty)
            {
                FechaTransaccion = fechaPago
            };

            miPrestamo.AgregarOtroAbono(payment);
            miPrestamo.Estado = Estados.CERRADO;

            //Acti
            decimal totalCobrar = miPrestamo.ObtenerTotalACobrar();

            //Asset
            Assert.AreEqual(Estados.CERRADO, miPrestamo.Estado);
            Assert.AreEqual(140000, totalCobrar);
        }
    }
}
