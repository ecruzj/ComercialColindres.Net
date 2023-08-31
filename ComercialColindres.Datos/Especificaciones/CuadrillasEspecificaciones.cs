using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class CuadrillasEspecificaciones
    {
        public static Specification<Cuadrillas> FiltrarPorPlanta(string valorBusqueda, int plantaId)
        {
            var especification = new Specification<Cuadrillas>(c => c.CuadrillaId != 0 && c.PlantaId == plantaId);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Cuadrillas>(c => c.NombreEncargado.ToUpper().Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }

        public static Specification<Cuadrillas> FiltrarCuadrilla(string valorBusqueda)
        {
            var especification = new Specification<Cuadrillas>(c => c.CuadrillaId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Cuadrillas>(c => c.NombreEncargado.ToUpper().Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
