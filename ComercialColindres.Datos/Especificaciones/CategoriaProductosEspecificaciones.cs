using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using System.Linq;

namespace ComercialColindres.Datos.Especificaciones
{
    public class CategoriaProductosEspecificaciones
    {
        public static Specification<CategoriaProductos> FiltrarProductosPorPlanta(string valorBusqueda, int plantaId)
        {
            var especification = new Specification<CategoriaProductos>(c => c.CategoriaProductoId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<CategoriaProductos>(c => c.Descripcion.ToUpper().Contains(valorBuscar)
                                                                                       && c.PrecioProductos.Any(p => p.PlantaId == plantaId));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
