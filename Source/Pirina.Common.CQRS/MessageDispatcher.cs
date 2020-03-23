using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.Transport.Providers;
using Glasswall.Kernel.Messaging;
using Glasswall.Kernel.Messaging.Dispatching;
using Glasswall.Kernel.Transport;

namespace Glasswall.Common.CQRS
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private ITransportDispatcher _dispatcher;
        private readonly Func<string> _queueFactory;
        private readonly IWriteOnlyTransportProvider _writeOnlyTransportProvider;

        public MessageDispatcher(IWriteOnlyTransportProvider writeOnlyTransportProvider, Func<string> queueFactory)
        {
            _writeOnlyTransportProvider = writeOnlyTransportProvider ?? throw new ArgumentNullException(nameof(writeOnlyTransportProvider));
            _queueFactory = queueFactory ?? throw new ArgumentNullException(nameof(queueFactory));
        }

        public async Task SendMessage(Message message, CancellationToken cancallationToken)
        {
            var dispatcher = await GetDispatcher();
            await dispatcher.SendMessage(message, cancallationToken);
        }

        private async Task<ITransportDispatcher> GetDispatcher()
        {
            return _dispatcher ?? (_dispatcher = await _writeOnlyTransportProvider.GetDispatcher(_queueFactory()));
        }
    }
}