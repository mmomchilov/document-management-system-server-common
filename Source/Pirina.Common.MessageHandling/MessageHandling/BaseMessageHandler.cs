using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Kernel.Extensions;
using Glasswall.Kernel.Logging;
using Glasswall.Kernel.Message.Handling;
using Glasswall.Kernel.Messaging;

namespace Glasswall.Common.MessageHandling.MessageHandling
{
    public abstract class BaseMessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : Message
    {
        protected IGWLogger<BaseMessageHandler<TMessage>> Logger;

        protected BaseMessageHandler(IGWLogger<BaseMessageHandler<TMessage>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(TMessage message, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            try
            {
                await InvokeInternal(message, cancellationToken);
            }
            catch(Exception e)
            {
                var handled = await OnError(e, message);
                if (!handled)
                    throw;
            }
        }

        protected virtual Task<bool> OnError(Exception e, TMessage message)
        {
            Logger.Log(LogLevel.Error, 0, String.Empty, e, (s, ex) => ExceptionExtensions.BuildExceptionStringRecursively(ex));
            return Task.FromResult(false);
        }

        protected abstract Task InvokeInternal(TMessage message, CancellationToken cancellationToken);
    }
}