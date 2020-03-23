using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glasswall.Kernel.Message.Handling;

namespace Glasswall.Common.MessageHandling.Factories
{
    public class HandlerResolverSettings : IHandlerResolverSettings
    {
        public IEnumerable<Assembly> LimitAssembliesTo => Enumerable.Empty<Assembly>();

        public bool HasCustomAssemblyList => false;
    }
}