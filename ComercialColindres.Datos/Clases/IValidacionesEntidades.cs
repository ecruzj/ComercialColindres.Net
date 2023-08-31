using System.Collections.Generic;

namespace ComercialColindres.Datos.Clases
{
    public interface IValidacionesEntidades
    {
        IEnumerable<string> GetValidationErrors();
        IEnumerable<string> GetValidationErrorsDelete();
    }
}
