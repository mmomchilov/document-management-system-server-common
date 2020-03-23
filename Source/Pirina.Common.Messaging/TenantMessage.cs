using System;
using Glasswall.Kernel.Messaging;

namespace Glasswall.Common.Messaging
{
    [Serializable]
    public class TenantMessage : Message
    {
        public Guid TenantId { get; }

        public TenantMessage(Guid transactionId, Guid tenantId) : base(transactionId)
        {
            TenantId = tenantId;
        }
    }
}