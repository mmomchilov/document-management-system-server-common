using System;

namespace Glasswall.Common.Messaging
{
    [Serializable]
    public class PayloadConnection
    {
        public string ConnectionString { get; }
        public string Object { get; }
        public string Key { get; }

        public PayloadConnection(string connectionString, string objectName, string key)
        {
            ConnectionString = connectionString;
            Object = objectName;
            Key = key;
        }
    }
}