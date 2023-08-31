namespace ComercialColindres.Datos.Entorno
{
    public class SqlCommandInfo
    {
        public SqlCommandInfo(string sql, object[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; set; }
        public object[] Parameters { get; set; }
    }
}
