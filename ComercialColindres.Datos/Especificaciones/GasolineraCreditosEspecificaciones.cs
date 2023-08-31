using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class GasolineraCreditosEspecificaciones
    {
        public static Specification<GasolineraCreditos> FiltrarBusqueda(string valorBusqueda)
        {
            var specification = new Specification<GasolineraCreditos>(c => c.CodigoGasCredito != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<GasolineraCreditos>(b => b.CodigoGasCredito.ToUpper().Contains(valorBuscar) ||
                b.Gasolinera.Descripcion.ToUpper().Contains(valorBuscar) || b.Gasolinera.NombreContacto.Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
