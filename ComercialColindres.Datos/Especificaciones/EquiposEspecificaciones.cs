using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class EquiposEspecificaciones
    {
        public static Specification<Equipos> FiltrarPorProveedor(string valorBusqueda, int proveedorId)
        {
            var especification = new Specification<Equipos>(c => c.EquipoId != 0 && c.ProveedorId == proveedorId);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Equipos>(c => c.PlacaCabezal.ToUpper().Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
