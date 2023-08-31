using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IGasolineraCreditoDomainServices
    {
        bool PuedeUtilizarGasCredito(GasolineraCreditos gasCredito, out string mensajeValidacion);
    }
}
