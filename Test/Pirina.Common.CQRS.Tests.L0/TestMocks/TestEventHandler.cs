using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.CQRS.Events;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Kernel.Logging;

namespace Glasswall.Common.CQRS.Tests.L0.TestMocks
{
    public class TestEventHandler : BaseEventMessageHandler<TestEvent>
    {
        public TestEventHandler(INotifier notifier, IGWLogger<BaseMessageHandler<TestEvent>> logger) 
            : base(notifier, logger)
        {
        }

        protected override Task InvokeInternal(TestEvent message, CancellationToken cancellationToken)
        {
            message.TestAction.Invoke();
            return base.InvokeInternal(message, cancellationToken);
        }
    }
}