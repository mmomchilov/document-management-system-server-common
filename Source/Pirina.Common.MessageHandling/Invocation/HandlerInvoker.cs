using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Kernel.Message.Handling;

namespace Glasswall.Common.MessageHandling.Invocation
{
    public class HandlerInvoker : IHandlerInvoker
    {
        public Task InvokeHandlers(IEnumerable<object> handlers, object message, CancellationToken cancallationToken)
        {
            return InvokeHandlersInternal(handlers, message, cancallationToken);
        }

        private Task InvokeHandlersInternal(IEnumerable<object> handlers, object message, CancellationToken cancallationToken)
        {
            if (handlers == null)
                throw new ArgumentNullException("message");

            var tasks = new List<Task>();

            var asList = handlers.ToList();

            asList.Aggregate(tasks, (c, handler) =>
            {
                var del = HandlerDelegateFactory.GetMessageHandlerDelegate(handler.GetType(), message.GetType());
                var delTask = del(handler, new[] { message, cancallationToken });
                tasks.Add(delTask);
                return c;
            });

            var whenAllTask = Task.WhenAll(tasks)
                .ContinueWith(t =>
                {
                    foreach (var h in asList)
                    {
                        var disposable = h as IDisposable;
                        disposable?.Dispose();
                    }

                    if (t.Exception != null)
                        throw t.Exception;
                });

            return whenAllTask;
        }
    }
}