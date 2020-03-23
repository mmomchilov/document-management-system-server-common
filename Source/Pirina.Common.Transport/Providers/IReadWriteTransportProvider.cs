using System.Threading.Tasks;
using Glasswall.Common.Transport.Context;
using Glasswall.Kernel.Transport;

namespace Glasswall.Common.Transport.Providers
{
    public interface IReadWriteTransportProvider
    {
        Task Setup<TContext>(TContext context) where TContext : TransportProviderContext;
        Task<ITransportDispatcher> GetDispatcher<TContext>(TContext context) where TContext : TransportProviderContext;
    }
}