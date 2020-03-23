using System;
using System.Diagnostics.CodeAnalysis;
using Glasswall.Kernel.DependencyResolver;
using Glasswall.Kernel.Serialisation;

namespace Glasswall.Common.Serialisation.Binary.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class BinarySerialisationInitialiser
    {
        public static IDependencyResolver AddBinarySerialisation(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            if (!dependencyResolver.Contains<ISerialiser, BinarySerialiser>())
                dependencyResolver.RegisterType<ISerialiser, BinarySerialiser>(Lifetime.Transient);

            return dependencyResolver;
        }
    }
}
