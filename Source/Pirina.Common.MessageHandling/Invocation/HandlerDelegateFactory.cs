using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Kernel.Reflection.Extensions;

namespace Glasswall.Common.MessageHandling.Invocation
{
    internal class HandlerDelegateFactory
    {
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Func<object, object[], Task>>
            MessageHandlerDelegatesCache = new ConcurrentDictionary<Tuple<Type, Type>, Func<object, object[], Task>>();

        public static Func<object, object[], Task> GetMessageHandlerDelegate(Type handlerType, Type commandType)
        {
            return MessageHandlerDelegatesCache.GetOrAdd(new Tuple<Type, Type>(handlerType, commandType),
                t => BuildMessageHandlerDelegateInternal(handlerType, commandType));
        }

        private static Func<object, object[], Task> BuildMessageHandlerDelegateInternal(Type targetType,
            Type commandType)
        {
            return targetType.GetAsyncInvoker("Handle", commandType, typeof(CancellationToken));
        }
    }
}