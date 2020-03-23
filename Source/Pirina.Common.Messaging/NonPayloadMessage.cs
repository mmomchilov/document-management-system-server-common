using System;

namespace Glasswall.Common.Messaging
{
    [Serializable]
    public class NonPayloadMessage : TenantMessage
    {
        public string ConnectionString { get; }

        public NonPayloadMessage(Guid transactionId, Guid tenantId, string connectionString) 
            : base(transactionId, tenantId)
        {
            ConnectionString = connectionString;
        }
    }
}