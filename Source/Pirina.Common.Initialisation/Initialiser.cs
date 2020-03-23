using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Glasswall.Kernel.DependencyResolver;
using Glasswall.Kernel.Initialisation;

namespace Glasswall.Common.Initialisation
{
    [ExcludeFromCodeCoverage]
    [Obsolete("Use extension method instead. ")]
    public abstract class Initialiser : IInitialiser
    {
        /// <summary>
        /// Gets the order the initialiser should run in.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public abstract byte Order { get; }

        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        public virtual bool AutoDiscoverable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Performs initialisation.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public async Task Initialise(IDependencyResolver dependencyResolver)
        {
            await this.InitialiseInternal(dependencyResolver);
        }

        /// <summary>
        /// Performs initialisation.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        protected abstract Task InitialiseInternal(IDependencyResolver dependencyResolver);
    }
}