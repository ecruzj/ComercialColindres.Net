using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class BoletasHumedadSpecification
    {
        public static Specification<BoletaHumedad> BoletasHumedadFilter(string searchValue)
        {
            var specification = new Specification<BoletaHumedad>(b => b.NumeroEnvio != string.Empty);
            var valueToFind = !string.IsNullOrWhiteSpace(searchValue) ? searchValue.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valueToFind))
            {
                var searchValueSpecification = new Specification<BoletaHumedad>(b => b.NumeroEnvio.ToUpper().Contains(valueToFind));
                specification &= searchValueSpecification;
            }

            return specification;
        }
    }
}
