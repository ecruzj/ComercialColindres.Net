using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class OrdenesCompraProductoEspecificaciones
    {
        public static Specification<OrdenesCompraProducto> FiltrarOrdenesCompraProductos(string valorBusqueda)
        {
            var specification = new Specification<OrdenesCompraProducto>(b => b.OrdenCompraNo != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<OrdenesCompraProducto>(b => b.OrdenCompraNo.ToUpper().Contains(valorBuscar) 
                || b.NoExoneracionDEI.ToUpper().Contains(valorBuscar)
                || b.ClientePlanta.NombrePlanta.ToUpper().Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }
    }
}
