using System;
using System.Collections.Generic;
using System.Linq;
using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ComercialColindres.ReglasNegocio.Utilities;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class OrdenCombustibleDomainServices : IOrdenCombustibleDomainServices
    {
        public bool PuedeActualizarOrdenCombustible(OrdenesCombustible ordenCombustible, decimal nuevoPrecioOrden, out string mensajeValidacion)
        {
            if (ordenCombustible == null) throw new ArgumentNullException("ordenCombustible");

            if (nuevoPrecioOrden <= 0)
            {
                mensajeValidacion = "El nuevo precio de la Orden de Combustible debe ser mayor a 0";
                return false;
            }

            if (ordenCombustible.Estado == Estados.CERRADO)
            {
                mensajeValidacion = "La Orden de Combustible ya ha sido Cerrada";
                return false;
            }

            if (ordenCombustible.EsOrdenPersonal)
            {
                mensajeValidacion = string.Empty;
                return true;
            }

            if (ordenCombustible.Boleta != null)
            {
                if (ordenCombustible.Boleta.Estado != Estados.ACTIVO)
                {
                    mensajeValidacion = "El estado de la Boleta debe ser ACTIVO";
                    return false;
                }

                var totalPagoBoleta = ordenCombustible.Boleta.ObtenerTotalAPagar();
                var aplicaNuevoMontoCombustible = ((totalPagoBoleta + ordenCombustible.Monto) - nuevoPrecioOrden) >= 0;

                if (!aplicaNuevoMontoCombustible)
                {
                    mensajeValidacion = "El Nuevo monto de Combustible excede el total de Pago de la Boleta";
                    return false;
                }
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryToApplyFuelOrderToBoleta(OrdenesCombustible ordenCombustible, Boletas boleta, out string mensajeValidacion)
        {            
            if (ordenCombustible == null) throw new ArgumentNullException("ordenCombustible");
            if (boleta == null) throw new ArgumentNullException("boleta");

            if (ordenCombustible.Monto > boleta.ObtenerTotalAPagar())
            {
                mensajeValidacion = $"El monto de la Orden de Combustible {ordenCombustible.CodigoFactura} es mayor al Total de Pago de la Boleta";
                return false;               
            }

            boleta.AgregarOrdenCombustible(ordenCombustible);

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryRemoveFuelOrderFromBoleta(OrdenesCombustible ordenCombustible, out string mensajeValidacion)
        {
            if (ordenCombustible == null) throw new ArgumentNullException("ordenCombustible");

            if (!ordenCombustible.EsOrdenPersonal)
            {
                if (ordenCombustible.Boleta == null)
                {
                    mensajeValidacion = "No Existe ninguna boleta asociada a la orden de Combustible";
                    return false;
                }

                if (ordenCombustible.Boleta.Estado != Estados.ACTIVO)
                {
                    mensajeValidacion = string.Format("La Boleta {0} al que esta asignada la Orden debe estar Activa", ordenCombustible.Boleta.CodigoBoleta);
                    return false;
                }

                ordenCombustible.RemoveBoletaFromFuel();
                mensajeValidacion = string.Empty;
                return true;
            }

            mensajeValidacion = string.Empty;
            return true;
        }

        public bool TryRemoveFuelOrder(OrdenesCombustible fuelOrder, out string errorMessage)
        {
            if (fuelOrder == null) throw new ArgumentNullException(nameof(fuelOrder));

            if (fuelOrder.IsAssignedToBoleta())
            {
                errorMessage = "La Orden de Combustible esta asignada a una Boleta";
                return false;
            }

            if (fuelOrder.GasolineraCredito.Estado != Estados.ACTIVO)
            {
                errorMessage = "La linea de Credito de Combustible debe estar Activo!";
                return false;
            }

            fuelOrder.RemoveOrderImg();                    

            errorMessage = string.Empty;
            return true;
        }

        public bool TryCreateManualPayments(OrdenesCombustible fuelOrder, List<FuelOrderManualPayment> manualPayments, out string errorMessage)
        {
            if (fuelOrder == null) throw new ArgumentNullException(nameof(fuelOrder));

            if (fuelOrder.Estado == Estados.CERRADO)
            {
                errorMessage = "La orden de combustible debe ser activa para agregar pagos manuales";
                return false;
            }

            if (fuelOrder.IsAssignedToBoleta())
            {
                errorMessage = "La orden de combustible está asignada a una boleta";
                return false;
            }

            decimal totalPayment = manualPayments.Sum(t => t.Amount);

            if (totalPayment > fuelOrder.Monto)
            {
                errorMessage = $"El total de pago L.{totalPayment} excede el monto de la orden de combustible";
                return false;
            }
                        
            List<FuelOrderManualPayment> currentPayments = fuelOrder.FuelOrderManualPayments.ToList();
            List<FuelOrderManualPayment> deletedItems = ListComparer.GetMissingElementsByIdentity(currentPayments, manualPayments, p => p.FuelOrderManualPaymentId);

            //Remove deleted manual payments
            foreach (FuelOrderManualPayment deletedPayment in deletedItems)
            {
                fuelOrder.RemoveManualPayment(deletedPayment);
            }

            foreach (FuelOrderManualPayment manualPayment in manualPayments)
            {
                IEnumerable<string> validationError = manualPayment.GetValidationErrors();

                if (validationError.Any())
                {
                    errorMessage = validationError.FirstOrDefault();
                    return false;
                }

                FuelOrderManualPayment currentManualPayment = fuelOrder.FuelOrderManualPayments.FirstOrDefault(m => m.FuelOrderManualPaymentId == manualPayment.FuelOrderManualPaymentId);

                if (currentManualPayment == null)
                {
                    //Add new manual payments
                    fuelOrder.AddManualPayment(manualPayment);
                }
                else
                {
                    //update item
                    currentManualPayment.UpdateManualPayment(manualPayment.Bank, manualPayment.BankReference, manualPayment.PaymentDate, manualPayment.WayToPay, manualPayment.Amount, manualPayment.Observations);
                }
            }

            //Update Fuel Order Status
            fuelOrder.UpdateStatus();

            errorMessage = string.Empty;
            return true;
        }
    }
}
