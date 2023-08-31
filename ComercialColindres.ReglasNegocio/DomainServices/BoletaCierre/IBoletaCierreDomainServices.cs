using ComercialColindres.Datos.Entorno.Entidades;

namespace ComercialColindres.ReglasNegocio.DomainServices.BoletaCierre
{
    public interface IBoletaCierreDomainServices
    {
        decimal BuildAveragePaymentMasive(decimal totalPaymentPending, decimal creditLineDeduction, Boletas boleta);
        bool RemoveBoletaCierre(Boletas boleta, out string errorMessage);
    }
}
