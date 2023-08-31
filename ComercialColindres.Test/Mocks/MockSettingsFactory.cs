using ComercialColindres.Datos.Entorno.DataCore.Setting;

namespace ComercialColindres.Test.Mocks
{
    public class MockSettingsFactory : ISettingFactory
    {
        public ISetting Create()
        {
            return new MockSettings();
        }
    }
}
