using System.Threading.Tasks;
using Glasswall.Common.Transport.Context;

namespace Glasswall.Common.Transport.Providers
{
    public interface IReadOnlyTransportProvider
    {
        Task Setup<TContext>(TContext context) where TContext : TransportProviderContext;
    }
}