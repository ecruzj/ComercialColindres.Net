using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class PrestamosEspecificaciones
    {
        public static Specification<Prestamos> FiltrarPrestamosBusqueda(string valorBusqueda)
        {
            var specification = new Specification<Prestamos>(b => b.CodigoPrestamo != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Prestamos>(b => b.CodigoPrestamo.ToUpper().Contains(valorBuscar) ||
                b.AutorizadoPor.ToUpper().Contains(valorBuscar) || b.Proveedor.Nombre.ToUpper().Contains(valorBuscar) || b.Observaciones.ToUpper().Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
