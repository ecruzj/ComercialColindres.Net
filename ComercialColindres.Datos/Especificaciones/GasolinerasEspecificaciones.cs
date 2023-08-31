using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using System.Linq;

namespace ComercialColindres.Datos.Especificaciones
{
    public class GasolinerasEspecificaciones
    {
        public static Specification<Gasolineras> FiltrarBusqueda(string valorBusqueda, bool conGasCredito)
        {

            var especification = conGasCredito ? new Specification<Gasolineras>(g => g.GasolineraId != 0 && g.GasolineraCreditos.Any()) :
                                                 new Specification<Gasolineras>(g => g.GasolineraId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Gasolineras>(g => g.NombreContacto.ToUpper().Contains(valorBuscar) ||
                g.Descripcion.ToUpper().Contains(valorBuscar) || g.TelefonoContacto.Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
