using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class PagoPrestamosDomainServices : IPagoPrestamosDomainServices
    {
        public void CloseLoan(IEnumerable<PagoPrestamos> pagosPrestamo)
        {
            if (pagosPrestamo == null || !pagosPrestamo.Any()) return;

            foreach (var pago in pagosPrestamo.Where(p => p.Prestamo.Estado == Estados.ACTIVO))
            {
                pago.Prestamo.CerrarPrestamo();
            }
        }

        public void TryReactiveLoanByBoleta(Boletas boleta)
        {
            if (!boleta.PagoPrestamos.Any()) return;

            List<PagoPrestamos> pagoPrestamos = boleta.PagoPrestamos.ToList();

            foreach (PagoPrestamos pagoPrestamo in pagoPrestamos)
            {
                pagoPrestamo.Prestamo.ReactiveLoan();                
            }
        }

        public bool TryValidateAbonosPrestamoPorBoleta(Prestamos prestamo, Boletas boleta, out string mensajeValidacion)
        {
            var totalAbono = prestamo.ObtenerTotalAbono();
            var totalAPagarBoleta = Math.Round(boleta.ObtenerTotalAPagar() + totalAbono, 2);

            if (totalAbono > totalAPagarBoleta)
            {
                mensajeValidacion = string.Format("El Total del Abono del prestamo {0} supera el total a Pagar de la Boleta {1}", prestamo.CodigoPrestamo, boleta.CodigoBoleta);
                return false;
            }
            
            return TryValidateAbonosPrestamo(prestamo, out mensajeValidacion);
        }

        public bool TryValidateAbonosPrestamo(Prestamos prestamo, out string mensajeValidacion)
        {
            var totalAbono = prestamo.ObtenerTotalAbono();
            var totalCobrar = prestamo.ObtenerTotalACobrar();

            if (totalAbono > totalCobrar)
            {
                mensajeValidacion = string.Format("El Total del Abono del Prestamo {0} supera el total a Pagar del mísmo", prestamo.CodigoPrestamo);
                return false;
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool PuedeRemoverAbonoPrestamo(PagoPrestamos abonoPrestamo, out string mensajeValidacion)
        {
            if (abonoPrestamo == null) throw new ArgumentNullException("abonoPrestamo");

            if (abonoPrestamo.Prestamo.Estado != Estados.ACTIVO)
            {
                mensajeValidacion = string.Format("Ya está CERRADO el Préstamo {0}", abonoPrestamo.Prestamo.CodigoPrestamo);
                return false;
            }

            mensajeValidacion = string.Empty;
            return true;
        }
    }
}
