using ComercialColindres.Datos.Entorno.Entidades;
using System.Collections.Generic;

namespace ComercialColindres.ReglasNegocio.DomainServices
{
    public interface IDescargadoresDomainServices
    {
        void TryAsignarDescargaProductoPorAdelantado(Boletas boleta, List<DescargasPorAdelantado> descargasPorAdelantado);
        bool TryActualizarPrecioDescargaProducto(Boletas boleta, decimal precioDescarga, out string mensajeValidacion);
        bool PuedeEliminarDescargaProducto(Descargadores descargaProducto, out string mensajeValidacion);
        bool TryRemoverDescargas(PagoDescargadores pagoDescarga, DescargasPorAdelantado descargaPorAdelanto, out string errorMessage);
        bool TryAssigneDescargaToPay(PagoDescargadores pagoDescargas, Boletas boleta, AjusteTipo ajusteTipo, DescargasPorAdelantado descargaPorAdelanto, string numeroEnvio, string codigoBoleta, decimal pagoDescarga, out string errorMessage);
    }
}
