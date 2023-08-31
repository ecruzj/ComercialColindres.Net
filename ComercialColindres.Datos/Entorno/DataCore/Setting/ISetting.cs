using System.Collections.Generic;

namespace ComercialColindres.Datos.Entorno.DataCore.Setting
{

    /// <summary>
    /// Base contract for gettting system's settings. 
    /// <remarks>
    /// This contract could be implemented using Sql Server,
    /// text files, xml files.
    /// </remarks>
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// Get the list of settings for the setting Id <paramref><name>settingId</name></paramref>.
        /// <remarks>
        /// The return type is a Dictionary where the key is the attributeId 
        /// and the value is the setting's value.
        /// </remarks>
        /// </summary>
        /// <param name="settingId">The setting Id to seach for.</param>
        /// <param name="facilityId">The facility where the settings belongs.</param>
        /// <returns>The collection of settings.</returns>
        Dictionary<string, string> GetSettingsById(string settingId, string facilityId);

        /// <summary>
        /// Gets the setting value for the setting Id <paramref><name>settingId</name></paramref>
        /// and the attribute Id <paramref><name>attibuteId</name></paramref>
        /// </summary>
        /// <param name="settingId">The setting Id.</param>
        /// <param name="attibuteId">The attribute Id.</param>
        /// <param name="facilityId">The facility where the settings belongs.</param>
        /// <returns>The setting value.</returns>
        string GetSettingByIdAndAttribute(string settingId, string attibuteId, string facilityId);

        /// <summary>
        /// Get the list of settings that are like the setting Id sent as parameter<paramref><name>settingId</name></paramref>.
        /// <remarks>
        /// The return type is a Dictionary where the key is the attributeId 
        /// and the value is the setting's value.
        /// </remarks>
        /// </summary>
        /// <param name="settingId">The setting Id to seach for.</param>
        /// <param name="facilityId">The facility where the settings belongs.</param>
        /// <returns>The collection of settings.</returns>
        Dictionary<string, string> GetSettingsBySimilarId(string settingId, string facilityId);

        /// <summary>
        /// Check if the attribute <see cref="attibuteId"/> is On (has a value of "1") for the setting <see cref="settingId"/>
        /// on facility <see cref="facilityId"/>.
        /// </summary>
        /// <param name="settingId">The setting id to check.</param>
        /// <param name="attibuteId">The attribute id to check.</param>
        /// <param name="facilityId">The facility where the attribute is on.</param>
        /// <returns>true or false wheter the setting is On.</returns>
        bool IsTheSettingOn(string settingId, string attibuteId, string facilityId);

        /// <summary>
        /// Check if the attribute <see cref="attributeValue"/> is On (has a value of "1")
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns>true or false wheter the setting is On.</returns>
        bool IsTheAttributeValueOn(string attributeValue);
    }
}
