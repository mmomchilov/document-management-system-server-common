using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Glasswall.Common.MessageHandling.Factories;
using Glasswall.Common.MessageHandling.Invocation;
using Glasswall.Kernel.DependencyResolver;
using Glasswall.Kernel.Message.Handling;

[assembly: InternalsVisibleTo("Glasswall.Common.CQRS.Tests.L0")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Glasswall.Common.MessageHandling.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MessageHandlingInitialiser
    {
        public static IDependencyResolver AddMessageHandling(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            if (!dependencyResolver.Contains<IHandlerResolver, HandlerResolver>())
                dependencyResolver.RegisterType<IHandlerResolver, HandlerResolver>(Lifetime.Transient);

            if (!dependencyResolver.Contains<IHandlerInvoker, HandlerInvoker>())
                dependencyResolver.RegisterType<IHandlerInvoker, HandlerInvoker>(Lifetime.Transient);

            if (!dependencyResolver.Contains<IHandlerResolverSettings, HandlerResolverSettings>())
                dependencyResolver.RegisterType<IHandlerResolverSettings, HandlerResolverSettings>(Lifetime.Transient);

            return dependencyResolver;
        }
    }
}