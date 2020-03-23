using System;
using System.Collections.Generic;
using System.Globalization;
using Glasswall.Kernel.Configuration;

namespace Glasswall.Common.Configuration
{
    public class EnvironmentVariableConfiguration : IConfiguration
    {
        public string this[string key]
        {
            get => GetValue(key);
            set => Environment.SetEnvironmentVariable(key, value);
        }

        public T GetValue<T>(string key)
        {
            if (!(typeof(T) == typeof(string) || typeof(T).IsPrimitive))
                throw new TypeAccessException("Only strings and primitive types can be stored in configuration");

            var value = this[key];
            return (T) Convert.ChangeType(value, typeof(T));
        }

        public void SetValue<T>(string key, T value)
        {
            if (!(typeof(T) == typeof(string) || typeof(T).IsPrimitive))
                throw new TypeAccessException("Only strings and primitive types can be stored in configuration");

             this[key] = Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private string GetValue(string key)
        {
            if (!EnvironmentVariableExists(key)) 
                throw new KeyNotFoundException($"A configuration item with the key '{key}' was not found");
            
            return Environment.GetEnvironmentVariable(key);
        }

        private bool EnvironmentVariableExists(string variable)
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable));
        }
    }
}