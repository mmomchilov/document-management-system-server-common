using System.Threading.Tasks;
using Glasswall.Common.Transport.Context;
using Glasswall.Kernel.Transport;

namespace Glasswall.Common.Transport.Providers
{
    public interface IWriteOnlyTransportProvider
    {
        Task<ITransportDispatcher> GetDispatcher<TContext>(TContext context) where TContext : TransportProviderContext;
    }
}