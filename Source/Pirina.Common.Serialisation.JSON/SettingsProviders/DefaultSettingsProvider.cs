using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Glasswall.Common.Serialisation.JSON.SettingsProviders
{
    public class DefaultSettingsProvider : SettingsProvider
    {
        protected override JsonSerializerSettings GetSettingsInternal()
        {
            var camelCase = new CamelCasePropertyNamesContractResolver();
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = camelCase;
            settings.TypeNameHandling = TypeNameHandling.None;
            return settings;
        }
    }
}
