using ComercialColindres.Datos.Entorno.Entidades;
using ComercialColindres.Datos.Recursos;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class AjusteBoletasSpecification
    {
        public static Specification<AjusteBoleta> FilterAjusteBoletas(string searchValue)
        {
            Specification<AjusteBoleta> specification = new Specification<AjusteBoleta>(a => a.AjusteBoletaId > 0);
            string findValue = !string.IsNullOrWhiteSpace(searchValue) ? searchValue.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(findValue))
            {
                var predicate = new Specification<AjusteBoleta>(b => b.Boleta.CodigoBoleta.ToUpper().Contains(findValue) ||
                    b.Boleta.NumeroEnvio.ToUpper().Contains(findValue) ||
                    b.Boleta.Motorista.ToUpper().Contains(findValue) ||
                    b.Boleta.PlacaEquipo.ToUpper().Contains(findValue) ||
                    b.Boleta.ClientePlanta.NombrePlanta.ToUpper().Contains(findValue) ||
                    b.Boleta.Proveedor.Nombre.ToUpper().Contains(findValue));

                specification &= predicate;
            }

            return specification;
        }
    }
}
