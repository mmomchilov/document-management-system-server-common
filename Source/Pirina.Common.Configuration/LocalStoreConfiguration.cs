using System;
using Glasswall.Kernel.Configuration;
using System.Collections.Generic;
using System.Globalization;

namespace Glasswall.Common.Configuration
{
    public class LocalStoreConfiguration : IConfiguration
    {
        private readonly Dictionary<string, string> _configurationStore = new Dictionary<string, string>();

        public string this[string key]
        {
            get => _configurationStore[key];
            set => _configurationStore[key] = value;
        }

        public T GetValue<T>(string key)
        {
            if (!(typeof(T) == typeof(string) || typeof(T).IsPrimitive))
                throw new TypeAccessException("Only strings and primitive types can be stored in configuration");

            var value = this[key];
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void SetValue<T>(string key, T value)
        {
            if (!(typeof(T) == typeof(string) || typeof(T).IsPrimitive))
                throw new TypeAccessException("Only strings and primitive types can be stored in configuration");

            this[key] = Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
