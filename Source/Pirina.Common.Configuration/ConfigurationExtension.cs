using System;
using System.Collections.Generic;
using System.Linq;
using Glasswall.Kernel.Configuration;

namespace Glasswall.Common.Configuration
{
    public static class ConfigurationExtension
    {
        public static void ValidateConfigurationContent(this IConfiguration configuration, Type configurationKeyStore)
        {
            var requiredConfigurationKeys = GetConfigurationValues(configurationKeyStore).ToList();
            if (requiredConfigurationKeys.Any(k => k==null))
                throw new ArgumentException("Configuration contains invalid data types");

            var errorList = CheckAllKeysAreInConfiguration(configuration, requiredConfigurationKeys).ToList();

            if (!errorList.Any()) return;

            var missingConfigurationItems = string.Join(", ", errorList);
            throw new ArgumentException($"Missing configuration for {missingConfigurationItems}");
        }

        private static IEnumerable<string> CheckAllKeysAreInConfiguration(IConfiguration configuration, IEnumerable<string> requiredConfigurationKeys)
        {
            var errorList = new List<string>();

            foreach (var keyValue in requiredConfigurationKeys)
            {
                try
                {
                    var configurationValue = configuration[keyValue];
                    if (string.IsNullOrEmpty(configurationValue))
                        throw new ArgumentException();
                }
                catch (Exception)
                {
                    errorList.Add(keyValue);
                }
            }

            return errorList;
        }

        private static IEnumerable<string> GetConfigurationValues(Type configurationKeyStore)
        {
            var fields = new Dictionary<string, object>();

            foreach (var prop in configurationKeyStore.GetFields())
                fields.Add(prop.Name, prop.GetValue(null));

            return fields.Values.Select(v => v as string);
        }
    }
}
