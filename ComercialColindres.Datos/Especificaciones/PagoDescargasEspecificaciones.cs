using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class PagoDescargasEspecificaciones
    {
        public static Specification<PagoDescargadores> FiltrarPagoDescargadoresBusqueda(string valorBusqueda)
        {
            var specification = new Specification<PagoDescargadores>(b => b.CodigoPagoDescarga != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<PagoDescargadores>(b => b.CodigoPagoDescarga.ToUpper().Contains(valorBuscar) ||
                b.Cuadrilla.NombreEncargado.ToUpper().Contains(valorBuscar) || b.Cuadrilla.ClientePlanta.NombrePlanta.Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
