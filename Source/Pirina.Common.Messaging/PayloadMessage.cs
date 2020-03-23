using System;

namespace Glasswall.Common.Messaging
{
    [Serializable]
    public class PayloadMessage : TenantMessage
    {
        public PayloadConnection PayloadConnection { get; }

        public PayloadMessage(Guid transactionId, Guid tenantId, PayloadConnection payloadConnection) : base(transactionId, tenantId)
        {
            PayloadConnection = payloadConnection;
        }
    }
}