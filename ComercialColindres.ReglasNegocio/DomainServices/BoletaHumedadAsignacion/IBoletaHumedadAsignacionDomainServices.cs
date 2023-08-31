using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IBoletaHumedadAsignacionDomainServices
    {
        void TryAssignBoletaHumidityToBoleta(Boletas boleta, BoletaHumedad boletaHumedad);
        void RemoveBoletasHumidityWithPaymentFromOthers(List<BoletaHumedadAsignacion> boletasHumidityAssignment, int boletaId);
    }
}
