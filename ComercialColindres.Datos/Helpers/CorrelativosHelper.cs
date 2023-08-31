using ComercialColindres.Datos.Entorno;
using ServidorCore.EntornoDatos;
using System.Data;
using System.Data.SqlClient;

namespace ComercialColindres.Datos.Helpers
{
    public abstract class CorrelativosHelper
    {
        public static string ObtenerCorrelativo(ComercialColindresContext context, int sucursalId, string codigoCorrelativo, string prefijoControl, bool actualizarCorrelativo = true)
        {
            var siguienteCorrelativo = string.Empty;
            var conexion = Utility.CadenaConexion("ComercialColindresContext");
            var sqlConnection = new SqlConnection(conexion);
            using (sqlConnection)
            {
                var sqlCommand = new SqlCommand("spObtenerCorrelativo", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.Add(new SqlParameter("@SucursalId", sucursalId));
                sqlCommand.Parameters.Add(new SqlParameter("@CodigoCorrelativo", codigoCorrelativo));
                sqlCommand.Parameters.Add(new SqlParameter("@ActualizarCorrelativo", actualizarCorrelativo));
                sqlCommand.Parameters.Add(new SqlParameter("@PrefijoControl", prefijoControl));
                sqlConnection.Open();
                var dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    siguienteCorrelativo = dataReader["SiguienteCorrelativo"].ToString();
                }
                sqlConnection.Close();
            }
            return siguienteCorrelativo;
        }
    }
}
