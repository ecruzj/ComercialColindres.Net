using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IBoletaHumedadPagoDomainServices
    {
        bool TryToAssignHumidityToBoletaForPayment(BoletaHumedad boletaHumedad, Boletas boleta, out string errorMessage);
        bool CanRemoveBoletaHumidityPayment(BoletaHumedadPago boletaHumidityPayment, out string errorMessage);
        void RemoveOldHimidityPayments(Boletas boleta, List<int> humidityPaymentsIdRequest);
    }
}
