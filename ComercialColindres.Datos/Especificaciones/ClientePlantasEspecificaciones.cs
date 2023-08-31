using ComercialColindres.Datos.Entorno.Entidades;
using ServidorCore.EntornoDatos;

namespace ComercialColindres.Datos.Especificaciones
{
    public class ClientePlantasEspecificaciones
    {
        public static Specification<ClientePlantas> FiltrarPorValor(string valorBusqueda)
        {
            var especification = new Specification<ClientePlantas>(c => c.PlantaId != 0);
            var valorBuscar = !string.IsNullOrWhiteSpace(valorBusqueda) ? valorBusqueda.ToUpper() : string.Empty;

            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                var valorBusquedaSpecification = new Specification<ClientePlantas>(c => c.NombrePlanta.ToUpper().Contains(valorBuscar));
                especification &= valorBusquedaSpecification;
            }

            return especification;
        }
    }
}
