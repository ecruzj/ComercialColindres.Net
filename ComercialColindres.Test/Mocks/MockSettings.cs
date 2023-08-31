using ComercialColindres.Datos.Entorno.DataCore.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercialColindres.Test.Mocks
{
    /// <summary>
    /// The mock settings.
    /// </summary>
    public class MockSettings : ISetting
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly List<MockConfiguracionesDetalle> _mockConfiguracionesDetalle = new List<MockConfiguracionesDetalle>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSettings"/> class.
        /// </summary>
        public MockSettings()
        {
            InicializarComponentes();
        }

        /// <summary>
        /// The inicializar componentes.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private void InicializarComponentes()
        {
            NumeroEnvioConfiguraciones();
        }

        public string GetSettingByIdAndAttribute(string settingId, string attibuteId, string facilityId)
        {
            var repository =
                _mockConfiguracionesDetalle.FirstOrDefault(q => q.CodigoConfiguracion == settingId && q.Atributo == attibuteId);

            if (repository != null)
            {
                return repository.Valor;
            }

            return string.Empty;
        }

        public Dictionary<string, string> GetSettingsById(string settingId, string facilityId)
        {
            throw new NotImplementedException();
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

        private void NumeroEnvioConfiguraciones()
        {
            List<MockConfiguracionesDetalle> numeroEnvioConfiguracion = new List<MockConfiguracionesDetalle>
            {
                new MockConfiguracionesDetalle { CodigoConfiguracion= "NumeroEnvio", Atributo = "1", Valor = "1"  }
            };

            _mockConfiguracionesDetalle.AddRange(numeroEnvioConfiguracion);
        }
    }
}
