using System;

namespace ComercialColindres.Datos.Entorno
{
    public class EntityMapping
    {
        public Type EntityType { get; set; }
        public string TableName { get; set; }
        public string TransactionTableName { get; set; }
    }
}
