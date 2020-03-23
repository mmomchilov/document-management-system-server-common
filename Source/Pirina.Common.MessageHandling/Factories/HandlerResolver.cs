using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Glasswall.Kernel.DependencyResolver;
using Glasswall.Kernel.Extensions;
using Glasswall.Kernel.Message.Handling;
using Glasswall.Kernel.Reflection.Reflection;

namespace Glasswall.Common.MessageHandling.Factories
{
    public class HandlerResolver : IHandlerResolver
    {
        private readonly IHandlerResolverSettings _factorySettings;
        private readonly IDependencyResolver _resolver;

        public HandlerResolver(IDependencyResolver resolver, IHandlerResolverSettings factorySettings)
        {
            _resolver = resolver;
            _factorySettings = factorySettings;
        }

        public ICollection<object> ResolveAllHandlersFor(Type targetType)
        {
            return ResolveHandlersFor(targetType, (t, s) => true);
        }

        public ICollection<object> ResolveHandlersFor(Type targetType,
            Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var handlerType = BuildHandlerType(targetType);
            return GetHandlersInternal(handlerType, filter);
        }

        protected virtual ICollection<object> GetHandlersInternal(Type handlerType,
            Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var handlers = ResolveHandlers(handlerType, filter);
            return handlers;
        }

        protected virtual Type BuildHandlerType(Type type)
        {
            var handlerType = typeof(IMessageHandler<>)
                .MakeGenericType(type);
            return handlerType;
        }

        protected virtual ICollection<object> ResolveHandlers(Type handlerType,
            Func<Type, IHandlerResolverSettings, bool> filter)
        {
            //object handler;
            var handlers = _resolver.ResolveAll(handlerType)
                .Where(h => filter(h.GetType(), _factorySettings))
                .ToList();

            if (handlers.Count > 0)
                return handlers;

            handlers = TryResolveFromAssemblies(handlerType, filter)
                .ToList();

            if (handlers.Count == 0)
                throw new InvalidOperationException($"No command handler of type: {handlerType.FullName} found.");

            return handlers;
        }

        private ICollection<object> TryResolveFromAssemblies(Type handlerType,
            Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var scannableAsseblies = _factorySettings.HasCustomAssemblyList
                ? _factorySettings.LimitAssembliesTo
                : AssemblyScanner.ScannableAssemblies;

            var implementors = ReflectionHelper.GetAllTypes(scannableAsseblies,
                t => !t.IsAbstract && !t.IsInterface && t.IsAssignableToGenericType(handlerType) &&
                     filter(t, _factorySettings));

            var root = new List<object>();
            var instances = implementors.Aggregate(root, (c, next) =>
            {
                c.Add(CreateInstance(next));
                return c;
            });
            return instances;
        }

        private object CreateInstance(Type type)
        {
            var ctors = type.GetConstructors();
            var ctor = ctors.OrderByDescending(c => c.GetParameters().Length).First();
            var parameters = ctor.GetParameters();
            var pars = new List<ParameterExpression>();
            var resolvedParams = new List<object>();
            foreach (var p in parameters)
            {
                var canResolve = _resolver.TryResolve(p.ParameterType, out var parInstance);
                if (!canResolve)
                    throw new InvalidOperationException(
                        $"Cannot resolve dependency for type: {type.Name}. Dependency type: {p.ParameterType.Name}");
                resolvedParams.Add(parInstance);
                pars.Add(Expression.Parameter(p.ParameterType));
            }

            var ctorExpression = Expression.New(ctor, pars);
            var lambda = Expression.Lambda(ctorExpression, pars);
            var instance = lambda.Compile().DynamicInvoke(resolvedParams.ToArray());
            return instance;
        }
    }
}