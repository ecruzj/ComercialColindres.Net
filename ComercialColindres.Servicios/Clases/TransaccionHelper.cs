using ComercialColindres.Datos.Entorno.Modelos;

namespace ComercialColindres.Servicios.Clases
{
    public class TransaccionHelper
    {
        public static TransaccionInformacion  CrearTransaccion(string usuarioId, string descripcionTransaccion)
        {
            return new TransaccionInformacion { Usuario = usuarioId, DescripcionTransaccion = descripcionTransaccion };
        }
    }
}
