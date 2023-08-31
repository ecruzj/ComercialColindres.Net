using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IBoletasDetalleDomainServices
    {
        string AgregarBoletaDetalle(Boletas boleta, List<Deducciones> deducciones, decimal montoDeduccion, string tipoDeduccion, string noCumento, 
                                    string observaciones);
        string ActualizarBoletaDetalle(Boletas boleta, List<Deducciones> deducciones, string tipoDeduccion, string noCumento, 
                                       string observaciones);

        string RegistrarBoletaDeducciones(Boletas boleta, List<Deducciones> listaDeducciones);

        List<BoletaDetalles> ObtenerBoletaDeducciones(Boletas boleta, List<Deducciones> listaDeducciones);
        string ActualizarEstadoBoleta(Boletas boleta);
        void RemoveBoletaDetalle(Boletas boleta);
    }
}
