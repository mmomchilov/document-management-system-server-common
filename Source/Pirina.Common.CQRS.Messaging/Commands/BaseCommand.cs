using System;
using Glasswall.Common.Messaging;

namespace Glasswall.Common.CQRS.Messaging.Commands
{
    [Serializable]
    public class BaseCommand : TenantMessage
    {
        public BaseCommand(Guid tenantId, Guid id, Guid correlationId) : base(id, tenantId)
        {
        }
    }
}