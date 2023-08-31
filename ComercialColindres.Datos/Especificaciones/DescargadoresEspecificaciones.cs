using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class DescargadoresEspecificaciones
    {
        public static Specification<Descargadores> FiltrarDescargas(string valorBusqueda)
        {
            var specification = new Specification<Descargadores>(d => d.DescargadaId != 0 && !d.EsDescargaPorAdelanto);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Descargadores>(d => d.Cuadrilla.NombreEncargado.ToUpper().Contains(valorBuscar) ||
                d.Cuadrilla.ClientePlanta.NombrePlanta.ToUpper().Contains(valorBuscar) || d.Boleta.NumeroEnvio.ToUpper().Contains(valorBuscar) ||
                d.Boleta.CodigoBoleta.ToUpper().Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
