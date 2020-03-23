using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glasswall.Common.MessageHandling.Factories;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Common.MessageHandling.Tests.L0.TestMocks;
using Glasswall.Kernel.DependencyResolver;
using Glasswall.Kernel.Logging;
using Glasswall.Kernel.Message.Handling;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.MessageHandling.Tests.L0.Factories
{
    [TestFixture]
    [Category("Glasswall.Common.MessageHandling.Tests.L0")]
    public class HandlerResolverTests
    {
        private Mock<IDependencyResolver> _mockDependenyResolver;
        private Mock<IHandlerResolverSettings> _mockHandlerResolverSettings;

        [SetUp]
        public void Init()
        {
            _mockDependenyResolver = new Mock<IDependencyResolver>();
            _mockHandlerResolverSettings = new Mock<IHandlerResolverSettings>();
        }

        [TestFixture]
        public class ResolveAllHandlersForMethod : HandlerResolverTests
        {
            [Test]
            public void Handler_Can_Be_Resolved_When_Registered_Without_Any_Assembly_Filtering()
            {
                //ARRANGE
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                _mockDependenyResolver.Setup(x => x.ResolveAll(typeof(IMessageHandler<TestMessage>)))
                    .Returns(new[] { new TestMessageHandler(() => { }, logger.Object, false) });

                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));

                //ASSERT
                Assert.That(handler.Single(), Is.AssignableFrom(typeof(TestMessageHandler)));
            }

            [Test]
            public void Handler_Can_Be_Resolved_When_Registered_With_Assembly_Filtering()
            {
                //ARRANGE
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                _mockDependenyResolver.Setup(x => x.ResolveAll(typeof(IMessageHandler<TestMessage>)))
                    .Returns(new[] { new TestMessageHandler(() => { }, logger.Object, false) });

                _mockHandlerResolverSettings.Setup(x => x.LimitAssembliesTo)
                    .Returns(new List<Assembly> {GetType().Assembly});
                _mockHandlerResolverSettings.Setup(x => x.HasCustomAssemblyList).Returns(true);

                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));

                //ASSERT
                Assert.That(handler.Single(), Is.AssignableFrom(typeof(TestMessageHandler)));
            }

            [Test]
            public void Handler_Can_Be_Resolved_When_Not_Registered_Without_Any_Assembly_Filtering()
            {
                //ARRANGE
                object action;
                object boolean;
                object logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>().Object;
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(Action), out action)).Returns(true);
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(bool), out boolean))
                    .Returns(true);
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(IGWLogger<BaseMessageHandler<TestMessage>>), out logger))
                    .Returns(true);
                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));

                //ASSERT
                Assert.That(handler.Single(), Is.AssignableFrom(typeof(TestMessageHandler)));
            }

            [Test]
            public void Handler_Can_Be_Resolved_When_Not_Registered_With_Assembly_Filtering()
            {
                //ARRANGE
                object action;
                object boolean;
                object logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>().Object;
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(Action), out action))
                    .Returns(true);
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(IGWLogger<BaseMessageHandler<TestMessage>>), out logger))
                    .Returns(true);
                _mockDependenyResolver.Setup(x => x.TryResolve(typeof(bool), out boolean))
                    .Returns(true);
                _mockHandlerResolverSettings.Setup(x => x.LimitAssembliesTo)
                    .Returns(new List<Assembly> { GetType().Assembly });
                _mockHandlerResolverSettings.Setup(x => x.HasCustomAssemblyList).Returns(true);

                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                var handler = handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));

                //ASSERT
                Assert.That(handler.Single(), Is.AssignableFrom(typeof(TestMessageHandler)));
            }

            [Test]
            public void
                If_No_Handlers_Can_Be_Found_For_Message_Type_Then_An_Invalid_Operation_Exception_Will_Be_Thrown()
            {
                //ARRANGE
                _mockHandlerResolverSettings.Setup(x => x.LimitAssembliesTo)
                    .Returns(new List<Assembly>());
                _mockHandlerResolverSettings.Setup(x => x.HasCustomAssemblyList).Returns(true);

                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                try
                {
                    handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));
                }
                catch (Exception exception)
                {
                    //ASSERT
                    Assert.That(exception, Is.TypeOf(typeof(InvalidOperationException)));
                    Assert.That(exception.Message,
                        Is.EqualTo(
                            $"No command handler of type: {typeof(IMessageHandler<TestMessage>).FullName} found."));
                }
            }

            [Test]
            public void If_Handler_Can_Not_Be_Resolved_Then_An_Invalid_Operation_Exception_Will_Be_Thrown()
            {
                //ARRANGE
                var handlerResolver =
                    new HandlerResolver(_mockDependenyResolver.Object, _mockHandlerResolverSettings.Object);

                //ACT
                try
                {
                    handlerResolver.ResolveAllHandlersFor(typeof(TestMessage));
                }
                catch (Exception exception)
                {
                    //ASSERT
                    var type = typeof(TestMessageHandler);
                    var ctors = type.GetConstructors();
                    var ctor = ctors.OrderByDescending(c => c.GetParameters().Length).First();
                    var parameters = ctor.GetParameters();

                    Assert.That(exception, Is.TypeOf(typeof(InvalidOperationException)));
                    Assert.That(exception.Message,
                        Is.EqualTo(
                            $"Cannot resolve dependency for type: {type.Name}. Dependency type: {parameters.First().ParameterType.Name}"));
                }
            }
        }
    }
}
