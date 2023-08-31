using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class OrdenesCombustibleEspecificaciones
    {
        public static Specification<OrdenesCombustible> FiltrarOrdenCombustibleBusqueda(string valorBusqueda)
        {
            var specification = new Specification<OrdenesCombustible>(b => b.OrdenCombustibleId > 0 && b.CodigoFactura != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<OrdenesCombustible>(b => b.CodigoFactura.ToUpper().Contains(valorBuscar) 
                                                                                       || b.GasolineraCredito.CodigoGasCredito.ToUpper().Contains(valorBuscar) 
                                                                                       || b.GasolineraCredito.Gasolinera.NombreContacto.Contains(valorBuscar) 
                                                                                       || b.GasolineraCredito.Gasolinera.Descripcion.Contains(valorBuscar)
                                                                                       || (b.Boleta != null && (b.Boleta.CodigoBoleta.ToUpper().Contains(valorBuscar)
                                                                                           || b.Boleta.NumeroEnvio.ToUpper().Contains(valorBuscar))));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
