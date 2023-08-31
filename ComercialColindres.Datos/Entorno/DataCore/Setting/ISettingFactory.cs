namespace ComercialColindres.Datos.Entorno.DataCore.Setting
{
    /// <summary>
    /// Base contract for Setting abstract factory
    /// </summary>
    public interface ISettingFactory
    {
        /// <summary>
        /// Create new Setting.
        /// </summary>
        /// <returns></returns>
        ISetting Create();
    }
}
