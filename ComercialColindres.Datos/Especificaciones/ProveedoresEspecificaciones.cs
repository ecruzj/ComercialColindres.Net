using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using System.Linq;

namespace ComercialColindres.Datos.Especificaciones
{
    public class ProveedoresEspecificaciones
    {
        public static Specification<Proveedores> FiltrarProveedoresBusqueda(string valorBusqueda)
        {
            var especification = new Specification<Proveedores>(o => o.ProveedorId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Proveedores>(o => o.RTN.ToUpper().Contains(valorBuscar) ||
                o.Nombre.ToUpper().Contains(valorBuscar) || o.CorreoElectronico.ToUpper().Contains(valorBuscar) || 
                o.Direccion.ToUpper().Contains(valorBuscar) || o.CedulaNo.ToUpper().Contains(valorBuscar) ||
                o.Equipos.Any(e => e.PlacaCabezal.ToUpper().Contains(valorBuscar)) || 
                o.CuentasBancarias.Any(c => c.CuentaNo.ToUpper().Contains(valorBuscar) || c.NombreAbonado.ToUpper().Contains(valorBuscar)) ||
                o.Conductores.Any(c => c.Nombre.ToUpper().Contains(valorBuscar) || c.Telefonos.Contains(valorBuscar)));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
