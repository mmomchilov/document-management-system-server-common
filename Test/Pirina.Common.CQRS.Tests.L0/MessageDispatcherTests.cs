using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.Transport.Providers;
using Glasswall.Kernel.Messaging;
using Glasswall.Kernel.Transport;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class MessageDispatcherTests
    {
        private const string QueueName = "Barry the Queue";
        private Mock<IWriteOnlyTransportProvider> _mockTransportProvider;
        private Mock<ITransportDispatcher> _mockDispatcher;
        private Func<string> _mockQueueFactory;

        [SetUp]
        public void Init()
        {
            _mockTransportProvider = new Mock<IWriteOnlyTransportProvider>();
            _mockDispatcher = new Mock<ITransportDispatcher>();
            _mockQueueFactory = () => QueueName;
        }

        [TestFixture]
        public class Constructor : MessageDispatcherTests
        {
            [Test]
            public void Null_Write_Only_Transport_Provider_Will_Throw_Argument_Null_Exception()
            {
                //Arrange

                //Act
                MessageDispatcher TestDelegate() => new MessageDispatcher(null, _mockQueueFactory);

                //Assert
                Assert.That(TestDelegate,
                    Throws.ArgumentNullException.With.Property("ParamName").EqualTo("writeOnlyTransportProvider"));
            }

            [Test]
            public void Null_Queue_Factory_Will_Throw_Argument_Null_Exception()
            {
                //Arrange

                //Act
                MessageDispatcher TestDelegate() => new MessageDispatcher(Mock.Of<IWriteOnlyTransportProvider>(), null);

                //Assert
                Assert.That(TestDelegate,
                    Throws.ArgumentNullException.With.Property("ParamName").EqualTo("queueFactory"));
            }

            [Test]
            public void Can_Be_Constructed()
            {
                //Arrange

                //Act
                var dispater = new MessageDispatcher(Mock.Of<IWriteOnlyTransportProvider>(), _mockQueueFactory);

                //Assert
                Assert.That(dispater, Is.Not.Null);
            }
        }

        [TestFixture]
        public class SendMessageMethod : MessageDispatcherTests
        {
            [Test]
            public async Task Can_Send_Message()
            {
                //Arrange
                var mockMessage = It.IsAny<Message>();
                _mockTransportProvider.Setup(x => x.GetDispatcher(QueueName))
                    .Returns(Task.FromResult(_mockDispatcher.Object));
                var dispatcher = new MessageDispatcher(_mockTransportProvider.Object, _mockQueueFactory);

                //Act
                await dispatcher.SendMessage(mockMessage, It.IsAny<CancellationToken>());

                //Assert
                _mockDispatcher.Verify(x => x.SendMessage(mockMessage, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Test]
            public void If_Transport_Dispatcher_Is_Null_A_Null_Reference_Exception_Will_Be_Thrown()
            {
                //Arrange
                _mockTransportProvider.Setup(x => x.GetDispatcher(QueueName))
                    .Returns(Task.FromResult<ITransportDispatcher>(null));
                var dispatcher = new MessageDispatcher(_mockTransportProvider.Object, _mockQueueFactory);

                //Act
                async Task TestDelegate() => await dispatcher.SendMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>());

                //Assert
                Assert.That(TestDelegate, Throws.TypeOf<NullReferenceException>());
            }

            [Test]
            public void Exception_Thrown_From_Transport_Send_Message_Will_Bubble_Up()
            {
                //Arrange
                const string exceptionMessage = "Test Exception";
                _mockTransportProvider.Setup(x => x.GetDispatcher(QueueName))
                    .Returns(Task.FromResult(_mockDispatcher.Object));
                _mockDispatcher.Setup(x => x.SendMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                    .Throws(new Exception(exceptionMessage));
                var dispatcher = new MessageDispatcher(_mockTransportProvider.Object, _mockQueueFactory);

                //Act
                async Task TestDelegate() => await dispatcher.SendMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>());

                //Assert
                Assert.That(TestDelegate, Throws.Exception.With.Message.EqualTo(exceptionMessage));
            }
        }
    }
}
