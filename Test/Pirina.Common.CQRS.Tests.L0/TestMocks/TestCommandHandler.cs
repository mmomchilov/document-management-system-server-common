using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.CQRS.Commands;
using Glasswall.Common.CQRS.Messaging.Events;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Kernel.Data.ORM;
using Glasswall.Kernel.Logging;
using Glasswall.Kernel.Messaging.Dispatching;

namespace Glasswall.Common.CQRS.Tests.L0.TestMocks
{
    public class TestCommandHandler : BaseCommandHandler<TestCommand>
    {
        public bool CreateOnSuccessEvent { get; set; }

        public TestCommandHandler(IDbContext dbContext, IMessageDispatcher dispatcher, IGWLogger<BaseMessageHandler<TestCommand>> logger) 
            : base(dbContext, dispatcher, logger)
        {
        }

        protected override Task HandleInternal(TestCommand command, CancellationToken cancellationToken)
        {
            command.TestAction.Invoke();
            return Task.CompletedTask;
        }

        protected override BaseEvent BuildEvent(TestCommand message)
        {
            if (CreateOnSuccessEvent)
                return base.BuildEvent(message);
            return null;
        }
    }
}
