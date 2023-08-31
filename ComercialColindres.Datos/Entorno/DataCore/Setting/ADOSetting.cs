using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ComercialColindres.Datos.Entorno.DataCore.Setting
{
    /// <summary>
    /// Implementation of contract  <see cref="CTS.NET.Infrastructure.Crosscutting.Setting.ISetting"/>
    /// using ADO.NET to get the setting from SQL Server.
    /// </summary>
    public sealed class ADOSetting : ISetting
    {
        public string GetSettingByIdAndAttribute(string settingId, string attibuteId, string facilityId)
        {
                var setting = string.Empty;

                const string sql = "SELECT Valor FROM ConfiguracionesDetalle " +
                                   "Where CodigoConfiguracion = @settingId and Atributo = @attibuteId";

                var connectionString = ConfigurationManager.ConnectionStrings["ComercialColindresContext"];
                if (connectionString != null)
                {
                    using (var ctsConexion = new SqlConnection(connectionString.ConnectionString))
                    {
                        using (var ctsCommand = ctsConexion.CreateCommand())
                        {
                            ctsCommand.CommandText = sql;
                            ctsCommand.Parameters.AddWithValue("@settingId", settingId);
                            ctsCommand.Parameters.AddWithValue("@attibuteId", attibuteId);

                            ctsConexion.Open();

                            using (var reader = ctsCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    setting = reader["Valor"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }

                return setting;
        }

        public Dictionary<string, string> GetSettingsById(string settingId, string facilityId)
        {
                var settings = new Dictionary<string, string>();

                const string sql = "SELECT Atributo, Valor FROM ConfiguracionesDetalle " +
                                   "Where ConfiguracionId = @settingId";

                var connectionString = ConfigurationManager.ConnectionStrings["ComercialColindresContext"];
                if (connectionString != null)
                {
                    using (var ctsConexion = new SqlConnection(connectionString.ConnectionString))
                    {
                        using (var ctsCommand = ctsConexion.CreateCommand())
                        {
                            ctsCommand.CommandText = sql;
                            ctsCommand.Parameters.AddWithValue("@settingId", settingId);

                            ctsConexion.Open();

                            using (var reader = ctsCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var atributo = reader["Atributo"].ToString();
                                    if (!settings.ContainsKey(atributo))
                                    {
                                        settings.Add(atributo, reader["Valor"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                return settings;
        }

        public Dictionary<string, string> GetSettingsBySimilarId(string settingId, string facilityId)
        {
            throw new NotImplementedException();
        }

        public bool IsTheAttributeValueOn(string attributeValue)
        {
            throw new NotImplementedException();
        }

        public bool IsTheSettingOn(string settingId, string attibuteId, string facilityId)
        {
            throw new NotImplementedException();
        }
    }
}
