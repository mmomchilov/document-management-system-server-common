using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.CQRS.Events;
using Glasswall.Common.CQRS.Tests.L0.TestMocks;
using Glasswall.Common.MessageHandling.Logging;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Kernel.Logging;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0.Events
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class BaseEventMessageHandlerTests
    {
        private Mock<INotifier> _mockNotifier;
        private Mock<IGWLogger<BaseMessageHandler<TestEvent>>> _mockEventLogger;

        [SetUp]
        public void Init()
        {
            _mockNotifier = new Mock<INotifier>();
            _mockEventLogger = new Mock<IGWLogger<BaseMessageHandler<TestEvent>>>();
        }

        [TestFixture]
        public class Constructor : BaseEventMessageHandlerTests
        {
            [Test]
            public void Null_Notifier_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                TestEventHandler TestDelegate() => new TestEventHandler(null, Mock.Of<IGWLogger<BaseMessageHandler<TestEvent>>>());

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("notifier"));
            }

            [Test]
            public void Null_Event_Logger_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                TestEventHandler TestDelegate() => new TestEventHandler(Mock.Of<INotifier>(), null);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("logger"));
            }

            [Test]
            public void Can_Be_Constructed_Successfully()
            {
                //Arrange

                //Act
                var handler = new TestEventHandler(Mock.Of<INotifier>(), Mock.Of<IGWLogger<BaseMessageHandler<TestEvent>>>());

                //Assert
                Assert.That(handler, Is.Not.Null);
            }
        }

        [TestFixture]
        public class InvokeInternalMethod : BaseEventMessageHandlerTests
        {
            [Test]
            public async Task Can_Be_Called_Successfully()
            {
                //Arrange
                var result = 0;
                var testEvent = new TestEvent(Guid.NewGuid(), Guid.NewGuid(), () => { result = 10; });
                var handler = new TestEventHandler(_mockNotifier.Object, Mock.Of<IGWLogger<BaseMessageHandler<TestEvent>>>());

                //Act
                await handler.Handle(testEvent, CancellationToken.None);

                //Assert
                Assert.That(result, Is.EqualTo(10));
                _mockNotifier.Verify(x => x.Notify(), Times.Once);
            }

            [Test]
            public void Failure_To_Notify_Will_Be_Logged()
            {
                //Arrange
                const string exceptionMessage = "Test Exception";
                var result = 0;
                var testEvent = new TestEvent(Guid.NewGuid(), Guid.NewGuid(), () => { result = 10; });
                var handler = new TestEventHandler(_mockNotifier.Object, _mockEventLogger.Object);
                _mockNotifier.
                    Setup(x => x.Notify()).
                    Throws(new Exception(exceptionMessage));

                //Act
                async Task TestDelegate() => await handler.Handle(testEvent, CancellationToken.None);

                //Assert
                Assert.That(TestDelegate, Throws.Exception);
                Assert.That(result, Is.EqualTo(10));
                _mockNotifier.Verify(x => x.Notify(), Times.Once);
                //_mockEventLogger.Verify(
                //    x => x.Log(SeverityLevel.Error, It.Is<EventId>(e => e == EventId.BaseMessageError), It.Is<Type>(t => t == typeof(TestEventHandler)), It.Is<Exception>(e => e.Message == exceptionMessage)), Times.Once);
            }
        }
    }
}
