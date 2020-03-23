using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.CQRS.Messaging.Events;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Kernel.Logging;

namespace Glasswall.Common.CQRS.Events
{
    public abstract class BaseEventMessageHandler<TMessage> : BaseMessageHandler<TMessage>
        where TMessage : BaseEvent
    {
        private readonly INotifier _notifier;
        
        protected BaseEventMessageHandler(INotifier notifier, IGWLogger<BaseMessageHandler<TMessage>> logger):base(logger)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        }

        protected override async Task InvokeInternal(TMessage message, CancellationToken cancellationToken)
        {
            await _notifier.Notify();
        }
    }
}