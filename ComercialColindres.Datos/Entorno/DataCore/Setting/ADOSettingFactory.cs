namespace ComercialColindres.Datos.Entorno.DataCore.Setting
{
    /// <summary>
    /// An ADO.Net base, setting factory.
    /// </summary>
    public class ADOSettingFactory : ISettingFactory
    {
        #region Implementation of ISettingFactory

        /// <summary>
        /// Create the ADO.Net source Setting.
        /// </summary>
        /// <returns>New ISetting based on ADO.NET infrastructure</returns>
        public ISetting Create()
        {
            return new ADOSetting();
        }

        #endregion
    }
}
