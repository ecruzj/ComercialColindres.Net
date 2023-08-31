using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComercialColindres.Datos.Entorno.DataCore.Setting
{
    /// <summary>
    /// Settings Factory
    /// </summary>
    public static class SettingFactory
    {
        private static ISettingFactory _currentSettingFactory;

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="settingFactory">Log factory to use</param>
        public static void SetCurrent(ISettingFactory settingFactory)
        {
            _currentSettingFactory = settingFactory;
        }

        /// <summary>
        /// Createt a new
        /// <paramref>
        ///     <name>CTS.NET.Infrastructure.Crosscutting.Setting.ISetting</name>
        /// </paramref>
        /// </summary>
        /// <returns>Created ISetting</returns>
        public static ISetting CreateSetting()
        {
            return (_currentSettingFactory != null) ? _currentSettingFactory.Create() : null;
        }
    }
}
