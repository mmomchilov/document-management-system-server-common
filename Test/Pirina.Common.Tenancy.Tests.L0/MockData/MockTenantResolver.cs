using System;
using System.Threading.Tasks;
using Glasswall.Common.Tenancy.Tenancy;
using Glasswall.Kernel.Tenancy;

namespace Glasswall.Common.Tenancy.Tests.L0.MockData
{
    internal class MockTenantResolver : TenantResolver<MockSource>
    {
        private readonly MockSource _source;
        private readonly Func<TenantResolutionContext, Task> _func;

        public MockTenantResolver(MockSource source, Func<TenantResolutionContext, Task> func)
        {
            this._source = source;
            this._func = func;
        }

        protected override async Task ResolveTenantInternal(MockSource source, TenantResolutionContext context)
        {
            await this._func(context);
        }

        protected override MockSource ResolveSource()
        {
            return this._source;
        }
    }
}