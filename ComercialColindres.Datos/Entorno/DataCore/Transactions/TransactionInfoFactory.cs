namespace ComercialColindres.Datos.Entorno
{
    public class TransactionInfoFactory
    {
        public static TransactionInfo CrearTransactionInfo(string user, string descripcionTransaccion)
        {
            return new TransactionInfo(user, descripcionTransaccion);
        }
    }
}
