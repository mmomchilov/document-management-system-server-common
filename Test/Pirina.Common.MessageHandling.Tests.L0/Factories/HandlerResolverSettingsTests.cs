using Glasswall.Common.MessageHandling.Factories;
using NUnit.Framework;

namespace Glasswall.Common.MessageHandling.Tests.L0.Factories
{
    [TestFixture]
    [Category("Glasswall.Common.MessageHandling.Tests.L0")]
    public class HandlerResolverSettingsTests
    {
        [TestFixture]
        public class LimitAssembliesToProperty : HandlerResolverSettingsTests
        {
            [Test]
            public void Will_Always_Return_An_Empty_Collection_Of_Assemblies_When_Using_Default_Constructor()
            {
                //Arrange
                var settings = new HandlerResolverSettings();

                //Act
                var result = settings.LimitAssembliesTo;

                //Assert
                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class HasCustomAssemblyListProperty : HandlerResolverSettingsTests
        {
            [Test]
            public void Will_Always_Return_False_When_Using_Default_Constructor()
            {
                //Arrange
                var settings = new HandlerResolverSettings();

                //Act
                var result = settings.HasCustomAssemblyList;

                //Assert
                Assert.That(result, Is.False);
            }
        }
    }
}
