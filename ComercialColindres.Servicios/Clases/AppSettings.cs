using System.Configuration;

namespace ComercialColindres.Servicios.Clases
{
    public class AppSettings
    {
        private static AppSettings _instance;

        public static AppSettings Instance => _instance ?? (_instance = new AppSettings());

        public virtual string FtpUser => GetStringAppSetting("FtpUserName");
        public virtual string FtpPass => GetStringAppSetting("FtpPassword");
        public virtual string FtpHostname => GetStringAppSetting("FtpHostname");
        public virtual int FtpPortNumber => int.Parse(GetStringAppSetting("FtpPortNumber"));

        private static string GetStringAppSetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName];
        }
    }
}
