using System;
using System.Threading.Tasks;
using Glasswall.Common.Tenancy.Tests.L0.MockData;
using Glasswall.Kernel.Tenancy;
using Glasswall.Kernel.Web.Authorisation;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.Tenancy.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Tenancy.Tests.L0")]
    public class TenantResolverTests
    {
        [Test]
        public async Task Resolve_tenant()
        {
            //ARRANGE
            var clientCredenials = new Mock<IBearerTokenContext>();
            var context = new TenantResolutionContext(new Kernel.Web.Endpoint("https://localhost/"), clientCredenials.Object);
            var tenantId = Guid.NewGuid();
            var source = new MockSource(tenantId);
            Func<TenantResolutionContext, Task> contextFunc = c =>
            {
                c.Resolved(new TenantDescriptor(tenantId));
                return Task.CompletedTask;
            };
            var resolver = new MockTenantResolver(source, contextFunc);
            //ACT
            await resolver.ResolveTenant(context, c => Task.CompletedTask);
            //ASSERT
            
            Assert.IsTrue(context.IsResolved);
            Assert.IsNotNull(context.TenantDescriptor);
            Assert.AreEqual(tenantId, context.TenantDescriptor.TenantId);
        }

        [Test]
        public async Task Cant_Resolve_tenant()
        {
            //ARRANGE
            var clientCredenials = new Mock<IBearerTokenContext>();
            var context = new TenantResolutionContext(new Kernel.Web.Endpoint("https://localhost/"), clientCredenials.Object);
            var tenantId = Guid.NewGuid();
            var source = new MockSource(tenantId);
            Func<TenantResolutionContext, Task> contextFunc = c =>
            {
                return Task.CompletedTask;

            };
            var resolver = new MockTenantResolver(source, contextFunc);
            
            //ACT
            await resolver.ResolveTenant(context, c => Task.CompletedTask);
            
            //ASSERT
            Assert.IsFalse(context.IsResolved);
            Assert.IsNull(context.TenantDescriptor);
        }
    }
}
