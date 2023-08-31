using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IOrdenCombustibleDomainServices
    {
        bool TryToApplyFuelOrderToBoleta(OrdenesCombustible ordenCombustible, Boletas boleta, out string mensajeValidacion);
        bool PuedeActualizarOrdenCombustible(OrdenesCombustible ordenCombustible, decimal nuevoPrecioOrden, out string mensajeValidacion);
        bool TryRemoveFuelOrderFromBoleta(OrdenesCombustible ordenCombustible, out string mensajeValidacion);
        bool TryRemoveFuelOrder(OrdenesCombustible ordenCombustible, out string errorMessage);
        bool TryCreateManualPayments(OrdenesCombustible fuelOrder, List<FuelOrderManualPayment> manualPayments, out string errorMessage);
    }
}
