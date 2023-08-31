using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IBoletaHumedadDomainServices
    {
        bool CanCreateBoletaHumedad(List<BoletaHumedad> boletasHumedad, string numeroEnvio, ClientePlantas destinationFacility, out string errorMessage);
        void TryToAssignOutStandingBoletaHumedadForPayment(BoletaHumedad outStandingBoleta, Boletas boleta, out string errorMessage);
        void CloseBoletaHumidity(List<BoletaHumedadPago> humidityPayments);
        bool TryToRemoveBoletaHumedad(BoletaHumedad boletaHumedad, out string errorMessage);
    }
}
