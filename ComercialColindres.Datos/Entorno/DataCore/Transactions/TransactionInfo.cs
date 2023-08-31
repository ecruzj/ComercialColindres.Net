using ComercialColindres.Datos.Entorno.Context;

namespace ComercialColindres.Datos.Entorno
{
    public class TransactionInfo : Entity
    {
        public TransactionInfo(string modificadoPor, string descripcionTransaccion)
                            :base(modificadoPor, descripcionTransaccion)
        {
            TipoTransaccion = descripcionTransaccion;
        }
    }
}
