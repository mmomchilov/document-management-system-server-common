using System;
using System.Collections.Generic;
using Glasswall.Kernel.Transport;

namespace Glasswall.Common.Transport
{
    public class TransportConfiguration : ITransportConfiguration
    {
        public TransportConfiguration()
        {
            Listeners = new List<IMessageListener>();
        }

        public int MaxDegreeOfParallelism { get; set; }
        public int ConcurrentListeners { get; set; }
        public TimeSpan ConsumerPeriod { get; set; }
        public ICollection<IMessageListener> Listeners { get; }
        public TransportConnection TransportConnection { get; set; }
        public bool IsTransactional { get; set; }
        public Mode TransportMode { get; set; }
    }
}