using System;
using System.Threading.Tasks;
using Glasswall.Kernel.Tenancy;

namespace Glasswall.Common.Tenancy.Tenancy
{
    /// <summary>
    /// Base class for tenant resolver
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public abstract class TenantResolver<TSource> : ITenantResolver<TSource>
    {
        /// <summary>
        /// Resolve tenant and continue with next resolver in the chain
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task ResolveTenant(TenantResolutionContext context, Func<TenantResolutionContext, Task> next)
        {
            var source = this.ResolveSource();
            await this.ResolveTenant(source, context);
            if (!context.IsResolved)
                await next(context);
        }

        /// <summary>
        /// Resolve tenant from given source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ResolveTenant(TSource source, TenantResolutionContext context)
        {
            if (source == null)
                return;
            await this.ResolveTenantInternal(source, context);
        }

        protected abstract Task ResolveTenantInternal(TSource source, TenantResolutionContext context);

        protected abstract TSource ResolveSource();
    }
}