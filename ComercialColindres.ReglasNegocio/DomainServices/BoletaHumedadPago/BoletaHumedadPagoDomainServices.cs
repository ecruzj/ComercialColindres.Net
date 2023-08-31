using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class BoletaHumedadPagoDomainServices : IBoletaHumedadPagoDomainServices
    {
        IBoletaHumedadAsignacionDomainServices _boletaHumedadAsignacionDomainServices;

        public BoletaHumedadPagoDomainServices(IBoletaHumedadAsignacionDomainServices boletaHumedadAsignacionDomainServices)
        {
            _boletaHumedadAsignacionDomainServices = boletaHumedadAsignacionDomainServices;
        }

        public bool TryToAssignHumidityToBoletaForPayment(BoletaHumedad boletaHumedad, Boletas boleta, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (boletaHumedad == null)
            {
                errorMessage = "No existe Boleta con Humedad";
                return false;
            }

            if (boleta == null)
            {
                errorMessage = "No existe la Boleta para Pagar";
                return false;
            }

            if (boletaHumedad.BoletaHumedadPago != null)
            {
                return true;
            }

            decimal totalToPay = boleta.ObtenerTotalAPagar();
            decimal humidityPrice = boletaHumedad.CalculateHumidityPriceForAssignment(boleta.PesoProducto, boleta.PrecioProductoCompra);

            if (totalToPay - humidityPrice < 0)
            {
                errorMessage = "El saldo de la humedad deja en negativo el pago de la Boleta";
                return false;
            }
            
            if (boletaHumedad.Estado == Estados.ACTIVO && humidityPrice > 0)
            {
                BoletaHumedadPago boletaHumedadPago = new BoletaHumedadPago(boleta, boletaHumedad);
                boleta.AddBoletaHumidityPay(boletaHumedadPago);
            }

            _boletaHumedadAsignacionDomainServices.TryAssignBoletaHumidityToBoleta(boleta, boletaHumedad);
            
            return true;
        }

        public bool CanRemoveBoletaHumidityPayment(BoletaHumedadPago boletaHumidityPayment, out string errorMessage)
        {

            if (boletaHumidityPayment == null)
            {
                errorMessage = "El Pago de la Boleta con Humedad no existe";
                return false;
            }

            if (boletaHumidityPayment.BoletaHumedad.Estado != Estados.ACTIVO)
            {
                errorMessage = "Solo puede eliminar Pagos de Boleta con Humedad cuando la Boleta con Humedad está Activa!";
                return false;
            }

            if (boletaHumidityPayment.Boleta.Estado != Estados.ACTIVO)
            {
                errorMessage = "Solo puede eliminar Pagos de Boleta con Humedad cuando la Boleta está Activa!";
                return false;
            }

            Boletas boleta = boletaHumidityPayment.Boleta;
            boleta.RemoveHumidityPayment(boletaHumidityPayment);

            errorMessage = string.Empty;
            return true;
        }

        public void RemoveOldHimidityPayments(Boletas boleta, List<int> humidityPaymentsIdRequest)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            List<BoletaHumedadPago> humidityPayments = boleta.BoletasHumedadPagos.ToList();

            foreach (BoletaHumedadPago humidityPaymet in humidityPayments.Where(h => h.BoletaHumedad.BoletaIngresada))
            {
                bool exists = humidityPaymentsIdRequest.Any(p => p == humidityPaymet.BoletaHumedadPagoId);

                if (exists) continue;
                boleta.BoletasHumedadPagos.Remove(humidityPaymet);
            }
        }
    }
}
