using System;
using Glasswall.Common.Messaging;

namespace Glasswall.Common.CQRS.Messaging.Events
{
    [Serializable]
    public abstract class BaseEvent : TenantMessage
    {
        public BaseEvent(Guid tenantId, Guid id) : base(tenantId, id)
        {
        }
    }
}
