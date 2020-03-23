using System;
using System.Diagnostics.CodeAnalysis;
using Glasswall.Kernel.DependencyResolver;

namespace Glasswall.Common.Logging
{
    [ExcludeFromCodeCoverage]
    public class LoggingInitialiser
    {
        [Obsolete("The ConsoleLoggingExtensions should be used instead", true)]
        protected IDependencyResolver InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            return dependencyResolver;
        }
    }
}
