using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface ILineasCreditoDeduccionesDomainServices
    {
        string EliminarDeduccionesPorBoleta(LineasCredito lineaCredito, int boletaCierreId);
        string ActualizarDeduccionPorBoleta(LineasCreditoDeducciones lineaCreditoDeduccion, BoletaCierres boletaCierre);
        string AplicarDeduccionCredito(LineasCredito lineaCredito, decimal montoDeduccion);
        bool AplicaRemoverDeduccionCredito(LineasCredito lineaCredito);
        IEnumerable<string> CrearDeduccionPorBoleta(BoletaCierres nuevaBoletaCierre, LineasCredito lineaCredito);
    }
}
