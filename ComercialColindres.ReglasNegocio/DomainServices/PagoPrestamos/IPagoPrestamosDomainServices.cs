using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IPagoPrestamosDomainServices
    {
        void CloseLoan(IEnumerable<PagoPrestamos> pagosPrestamo);
        void TryReactiveLoanByBoleta(Boletas boleta);
        bool TryValidateAbonosPrestamoPorBoleta(Prestamos prestamo, Boletas boleta, out string mensajeValidacion);
        bool TryValidateAbonosPrestamo(Prestamos prestamo, out string mensajeValidacion);
        bool PuedeRemoverAbonoPrestamo(PagoPrestamos abonoPrestamo, out string mensajeValidacion);
    }
}
