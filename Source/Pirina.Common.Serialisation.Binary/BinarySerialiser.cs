using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Glasswall.Kernel.Serialisation;

namespace Glasswall.Common.Serialisation.Binary
{
    public class BinarySerialiser : ISerialiser
    {
        private readonly BinaryFormatter _binaryFormatter;

        public BinarySerialiser()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public Task<Stream> Serialise(object obj)
        {
            var memoryStream = new MemoryStream();
            
            _binaryFormatter.Serialize(memoryStream, obj);
            return Task.FromResult((Stream)memoryStream);
        }

        public Task<object> Deserialise(Stream stream)
        {
            var obj = _binaryFormatter.Deserialize(stream);
            return Task.FromResult(obj);
        }

        public Task<T> Deserialise<T>(Stream stream)
        {
            var obj = Deserialise(stream).Result;
            return Task.FromResult((T)obj);
        }
    }
}
