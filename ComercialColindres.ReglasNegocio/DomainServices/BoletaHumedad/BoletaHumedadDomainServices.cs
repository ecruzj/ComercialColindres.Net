using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class BoletaHumedadDomainServices : IBoletaHumedadDomainServices
    {
        IBoletaHumedadPagoDomainServices _boletaHumedadPagoDomainServices;

        public BoletaHumedadDomainServices(IBoletaHumedadPagoDomainServices boletaHumedadPagoDomainServices)
        {
            _boletaHumedadPagoDomainServices = boletaHumedadPagoDomainServices;
        }
        
        public bool CanCreateBoletaHumedad(List<BoletaHumedad> boletasHumedad, string numeroEnvio, ClientePlantas destinationFacility, out string errorMessage)
        {
            bool existBoletaHumedad = boletasHumedad.Any(h => h.NumeroEnvio == numeroEnvio && h.PlantaId == destinationFacility.PlantaId);

            if (existBoletaHumedad)
            {
                errorMessage = "El número de envío ya existe ingresado en las Humedades!";
                return false;
            }
            
            errorMessage = string.Empty;
            return true;
        }

        public void TryToAssignOutStandingBoletaHumedadForPayment(BoletaHumedad outStandingBoleta, Boletas boleta, out string errorMessage)
        {
            if (boleta == null) throw new ArgumentNullException(nameof(boleta));

            errorMessage = string.Empty;
            if (outStandingBoleta == null || outStandingBoleta.BoletaHumedadPago != null) return;

            _boletaHumedadPagoDomainServices.TryToAssignHumidityToBoletaForPayment(outStandingBoleta, boleta, out errorMessage);
        }

        public void CloseBoletaHumidity(List<BoletaHumedadPago> humidityPayments)
        {
            if (humidityPayments == null) throw new ArgumentNullException(nameof(humidityPayments));

            foreach (BoletaHumedadPago boletaHumidityPayment in humidityPayments)
            {
                boletaHumidityPayment.BoletaHumedad.CloseBoletaHumidity();
            }
        }

        public bool TryToRemoveBoletaHumedad(BoletaHumedad boletaHumedad, out string errorMessage)
        {
            if (boletaHumedad == null) throw new ArgumentNullException(nameof(boletaHumedad));

            if (boletaHumedad.BoletaHumedadPago != null)
            {
                errorMessage = "La Humedad ya está asignada a una Boleta para Pago";
                return false;
            }

            boletaHumedad.RemoveAssignment();

            errorMessage = string.Empty;
            return true;            
        }

        private BoletaHumedad CreateBoletaHumedad(string numeroEnvio, ClientePlantas destinationFacility, bool boletaEntered, decimal averageHumidy, decimal tolerancePercentage, DateTime fechaHumedad)
        {
            return new BoletaHumedad(numeroEnvio, destinationFacility, boletaEntered, averageHumidy, tolerancePercentage, fechaHumedad);
        }
    }
}
