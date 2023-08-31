using System;

namespace ComercialColindres.Datos.Clases
{
    public interface IAuditedEntity
    {
        DateTime FechaTransaccion { get; set; }
        string ModificadoPor { get; set; }
        string DescripcionTransaccion { get; set; }
    }
}
