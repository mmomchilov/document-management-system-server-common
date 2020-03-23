using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Glasswall.Kernel.Serialisation;
using Newtonsoft.Json;

namespace Glasswall.Common.Serialisation.JSON
{
    public class NSJsonSerializer : IJsonSerialiser
    {
        #region fields

        private readonly JsonSerializerSettings jsonSerializerSettings;
        
        #endregion

        #region Constructors

        public NSJsonSerializer(ISerialisationSettingsProvider<JsonSerializerSettings> jsonSerializerSettings)
        {
            if (jsonSerializerSettings == null)
                throw new ArgumentNullException(nameof(jsonSerializerSettings));

            this.jsonSerializerSettings = jsonSerializerSettings.GetSettings().Settings;
        }

        #endregion

        public async Task<string> SerialiseToJson(object obj)
        {
            var stream = await ((ISerialiser)this).Serialise(obj);
            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public async Task<object> DeserialiseFromJson(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return await ((ISerialiser)this).Deserialise(ms);
            }
        }

        public async Task<T> DeserialiseFromJson<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return await ((ISerialiser)this).Deserialise<T>(ms);
            }
        }

        Task<Stream> ISerialiser.Serialise(object obj)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var ser = JsonSerializer.Create(this.jsonSerializerSettings);
            ser.Serialize(writer, obj);
            writer.Flush();
            ms.Position = 0;
            return Task.FromResult<Stream>(ms);
        }

        Task<object> ISerialiser.Deserialise(Stream stream)
        {
            using (var jsonReader = new JsonTextReader(new StreamReader(stream)))
            {
                var ser = JsonSerializer.Create(this.jsonSerializerSettings);
                var result = ser.Deserialize(jsonReader);
                return Task.FromResult(result);
            }
        }

        Task<T> ISerialiser.Deserialise<T>(Stream stream)
        {
            using (var jsonReader = new JsonTextReader(new StreamReader(stream)))
            {
                var ser = JsonSerializer.Create(this.jsonSerializerSettings);
                var result = ser.Deserialize(jsonReader, typeof(T));
                return Task.FromResult((T)result);
            }
        }
    }
}