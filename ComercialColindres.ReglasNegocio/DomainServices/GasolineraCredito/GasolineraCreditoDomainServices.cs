using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public class GasolineraCreditoDomainServices : IGasolineraCreditoDomainServices
    {
        public bool PuedeUtilizarGasCredito(GasolineraCreditos gasCredito, out string mensajeValidacion)
        {
            if (gasCredito == null)
            {
                mensajeValidacion = "GasCreditoId NO Existe";
                return false;
            }

            if (!gasCredito.EsCreditoActual)
            {
                mensajeValidacion = string.Format("El Crédito de Gasolina {0} no es el actual", gasCredito.CodigoGasCredito);
                return false;
            }

            mensajeValidacion = string.Empty;
            return true;
        }
    }
}
