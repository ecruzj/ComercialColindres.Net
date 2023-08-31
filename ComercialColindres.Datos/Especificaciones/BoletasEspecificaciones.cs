using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;
using System.Linq;

namespace ComercialColindres.Datos.Especificaciones
{
    public class BoletasEspecificaciones
    {
        public static Specification<Boletas> FiltrarBoletasBusqueda(string valorBusqueda)
        {
            var specification = new Specification<Boletas>(b => b.CodigoBoleta != string.Empty);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Boletas>(b => b.CodigoBoleta.ToUpper().Contains(valorBuscar) ||
                b.NumeroEnvio.ToUpper().Contains(valorBuscar) || 
                b.Motorista.ToUpper().Contains(valorBuscar) || 
                b.PlacaEquipo.ToUpper().Contains(valorBuscar) ||
                b.ClientePlanta.NombrePlanta.ToUpper().Contains(valorBuscar) || 
                b.Proveedor.Nombre.ToUpper().Contains(valorBuscar));
                specification &= valorBusquedaSpecification;
            }

            return specification;
        }

        public static Specification<Boletas> FiltrarPorPlantaBoletaFacturar(string valorBusqueda, int plantaId)
        {
            var especification = new Specification<Boletas>(c => c.PlantaId != 0 && c.PlantaId == plantaId);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<Boletas>(c => (c.CodigoBoleta.ToUpper().Contains(valorBuscar) ||
                                                                            c.NumeroEnvio.ToUpper().Contains(valorBuscar)) && !c.FacturaDetalleBoletas.Any());
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }

        public static Specification<Boletas> FiltrarBoletasPendientesDeFacturar(int plantaId)
        {
            var especification = new Specification<Boletas>(c => c.PlantaId != 0 && c.PlantaId == plantaId);

            var valorBusquedaSpecification = new Specification<Boletas>(c => !c.FacturaDetalleBoletas.Any());
            especification &= valorBusquedaSpecification;

            return especification;
        }
    }
}
