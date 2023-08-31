using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class ConductoresEspecificaciones
    {
        public static Specification<Conductores> FiltrarPorProveedor(string valorBusqueda, int proveedorId)
        {
            var especification = new Specification<Conductores>(c => c.ConductorId != 0 && c.ProveedorId == proveedorId);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Conductores>(c => c.Nombre.ToUpper().Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
