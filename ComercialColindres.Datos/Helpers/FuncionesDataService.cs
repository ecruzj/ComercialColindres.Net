using ComercialColindres.Datos.Entorno;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ComercialColindres.Datos.Helpers
{
    public class FuncionesDataService
    {
        public static void CalcularSaldoFactura(ComercialColindresContext unidadDeTrabajo, int facturaId)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@FacturaId", facturaId)
            };

            unidadDeTrabajo.Database.ExecuteSqlCommand("EXEC spCalcularSaldoFactura @FacturaId", parametros.ToArray());
        }
    }
}
