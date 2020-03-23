using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.CQRS.Messaging.Events;
using Glasswall.Common.CQRS.Tests.L0.TestMocks;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Kernel.Data.ORM;
using Glasswall.Kernel.Logging;
using Glasswall.Kernel.Messaging;
using Glasswall.Kernel.Messaging.Dispatching;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0.Commands
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class BaseCommandHandlerTests
    {
        private Mock<IMessageDispatcher> _mockMessageDispatcher;
        private Mock<IGWLogger<BaseMessageHandler<TestCommand>>> _mockEventLogger;

        [SetUp]
        public void Init()
        {
            _mockMessageDispatcher = new Mock<IMessageDispatcher>();
            _mockEventLogger = new Mock<IGWLogger<BaseMessageHandler<TestCommand>>>();
        }

        [TestFixture]
        public class Constructor : BaseCommandHandlerTests
        {
            [Test]
            public void Null_Db_Context_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                TestCommandHandler TestDelegate() =>
                    new TestCommandHandler(null, Mock.Of<IMessageDispatcher>(), Mock.Of<IGWLogger<BaseMessageHandler<TestCommand>>>());

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dbContext"));
            }

            [Test]
            public void Null_Message_Dispatcher_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                TestCommandHandler TestDelegate() =>
                    new TestCommandHandler(Mock.Of<IDbContext>(), null, Mock.Of<IGWLogger<BaseMessageHandler<TestCommand>>>());

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dispatcher"));
            }

            [Test]
            public void Null_Event_Logger_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                TestCommandHandler TestDelegate() =>
                    new TestCommandHandler(Mock.Of<IDbContext>(), Mock.Of<IMessageDispatcher>(), null);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("logger"));
            }

            [Test]
            public void Can_Be_Successfully_Constructed()
            {
                //Arrange

                //Act
                var handler = new TestCommandHandler(Mock.Of<IDbContext>(), Mock.Of<IMessageDispatcher>(), Mock.Of<IGWLogger<BaseMessageHandler<TestCommand>>>());

                //Assert
                Assert.That(handler, Is.Not.Null);
            }
        }

        [TestFixture]
        public class InvokeInternalMethod : BaseCommandHandlerTests
        {
            [Test]
            public async Task Can_Be_Called_Successfully()
            {
                //Arrange
                var result = 0;
                var testCommand =
                    new TestCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), () => { result = 10; });
                var handler =
                    new TestCommandHandler(Mock.Of<IDbContext>(), _mockMessageDispatcher.Object, Mock.Of<IGWLogger<BaseMessageHandler<TestCommand>>>())
                    {
                        CreateOnSuccessEvent = true
                    };

                //Act
                await handler.Handle(testCommand, CancellationToken.None);

                //Assert
                Assert.That(result, Is.EqualTo(10));
                _mockMessageDispatcher.Verify(x => x.SendMessage(It.IsAny<BaseEvent>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Test]
            public async Task Failure_To_Create_On_Success_Event_Will_Result_In_Argument_Null_Exception()
            {
                //Arrange
                var result = 0;
                var testCommand =
                    new TestCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), () => { result = 10; });
                var handler =
                    new TestCommandHandler(Mock.Of<IDbContext>(), _mockMessageDispatcher.Object, Mock.Of<IGWLogger<BaseMessageHandler<TestCommand>>>())
                    {
                        CreateOnSuccessEvent = false
                    };

                //Act
                await handler.Handle(testCommand, CancellationToken.None);

                //Assert
                Assert.That(result, Is.EqualTo(10));
                _mockMessageDispatcher.Verify(
                    x => x.SendMessage(It.IsAny<OnExceptionEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Test]
            public async Task Failure_To_Send_On_Exception_Event_Will_Log_The_Exception()
            {
                //Arrange
                const string exceptionMessage = "Test Exception";
                var result = 0;
                var testCommand =
                    new TestCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), () => { result = 10; });
                var handler =
                    new TestCommandHandler(Mock.Of<IDbContext>(), _mockMessageDispatcher.Object, _mockEventLogger.Object)
                    {
                        CreateOnSuccessEvent = false
                    };
                _mockMessageDispatcher.Setup(x => x.SendMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                    .Throws(new Exception(exceptionMessage));

                //Act
                await handler.Handle(testCommand, CancellationToken.None);

                //Assert
                Assert.That(result, Is.EqualTo(10));
                _mockMessageDispatcher.Verify(
                    x => x.SendMessage(It.IsAny<OnExceptionEvent>(), It.IsAny<CancellationToken>()), Times.Once);
                //_mockEventLogger.Verify(x => x.Log(SeverityLevel.Error, It.Is<EventId>(i => i == EventId.BaseMessageError), It.Is<Type>(t => t == typeof(TestCommandHandler)), It.Is<Exception>(e => e.Message == exceptionMessage)));
            }
        }
    }
}
