using System;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Common.MessageHandling.Tests.L0.TestMocks;
using Glasswall.Kernel.Logging;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.MessageHandling.Tests.L0.MessageHandling
{
    [TestFixture]
    [Category("Glasswall.Common.MessageHandling.Tests.L0")]
    public class BaseMessageHandlerTests
    {
        [TestFixture]
        public class Constructor : BaseMessageHandlerTests
        {
            [Test]
            public void If_Event_Logger_Is_Null_Then_An_Argument_Null_Exception_Will_Be_Thrown()
            {
                //Arrange

                //Act
                TestMessageHandler TestDelegate() => new TestMessageHandler(() => { }, null, false);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("logger"));
            }
        }

        [TestFixture]
        public class HandlerTests : BaseMessageHandlerTests
        {
            [Test]
            public void If_Message_Is_Null_Then_An_Argument_Null_Exception_Will_Be_Thrown()
            {
                // Arrange
                var testMessageHandler = new TestMessageHandler(
                    action: null, 
                    logger: Mock.Of<IGWLogger<BaseMessageHandler<TestMessage>>>(),
                    throwException: false);

                Task TestDelegate() => testMessageHandler.Handle(message: null, cancellationToken: CancellationToken.None);

                // Act
                // Assert
                var exception = Assert.ThrowsAsync<ArgumentNullException>(TestDelegate, "null message should result in exception being thrown");
                Assert.That(exception.ParamName, Is.EqualTo("message"));
            }
        }

        [TestFixture]
        public class OnErrorMethod : BaseMessageHandlerTests
        {
            [Test]
            public void If_Override_Prevents_Throwing_The_Caught_Exception_It_Will_Be_Shallowed()
            {
                //Arrange
                var handler = new TestMessageHandler(() => throw new Exception("This Should be Shallowed"),
                    Mock.Of<IGWLogger<BaseMessageHandler<TestMessage>>>(), true);

                //Act
                async Task TestDelegate() => await handler.Handle(new TestMessage(It.IsAny<Guid>(), It.IsAny<Guid>()), CancellationToken.None);

                //Assert
                Assert.That(TestDelegate, Throws.Nothing);
            }
            [Test]
            public void If_Override_Does_Not_Prevents_Throwing_The_Caught_Exception_It_Will_Be_ReThrown()
            {
                //Arrange
                const string exceptionMessage = "Re-thrown Exception";
                var handler = new TestMessageHandler(() => throw new Exception(exceptionMessage),
                    Mock.Of<IGWLogger<BaseMessageHandler<TestMessage>>>(), false);

                //Act
                async Task TestDelegate() => await handler.Handle(new TestMessage(It.IsAny<Guid>(), It.IsAny<Guid>()), CancellationToken.None);

                //Assert
                Assert.That(TestDelegate, Throws.Exception.With.Message.EqualTo(exceptionMessage));
            }

        }
    }
}
