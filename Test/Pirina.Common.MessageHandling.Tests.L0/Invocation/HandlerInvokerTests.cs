using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Glasswall.Common.MessageHandling.Invocation;
using Glasswall.Common.MessageHandling.MessageHandling;
using Glasswall.Common.MessageHandling.Tests.L0.TestMocks;
using Glasswall.Kernel.Logging;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.MessageHandling.Tests.L0.Invocation
{
    [TestFixture]
    [Category("Glasswall.Common.MessageHandling.Tests.L0")]
    public class HandlerInvokerTests
    {
        [TestFixture]
        [Category("Glasswall.Common.MessageHandling.Tests.L0")]
        public class InvokeHandlersMethod
        {
            [Test]
            public async Task Exception_Thrown_Within_Handler_The_Exception_Will_Bubble_Though_To_Invoker()
            {
                //ARRANGE
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                const string errorMessage = "Hello World! I am an error message";
                var command = new TestMessage(Guid.NewGuid(), Guid.NewGuid());
                var handler = new TestMessageHandler(() => throw new Exception(errorMessage), logger.Object, false);
                var handlerInvoker = new HandlerInvoker();

                //ACT
                try
                {
                    await handlerInvoker.InvokeHandlers(new[] {handler}, command, CancellationToken.None);
                }
                catch (AggregateException ae)
                {
                    //ASSERT
                    var exception = ae.InnerExceptions.First();
                    Assert.That(exception, Is.TypeOf<Exception>());
                    Assert.That(exception.Message, Is.EqualTo(errorMessage));
                }
            }

            [Test]
            public void If_Handler_Is_Null_Argument_Null_Exception_Is_Thrown()
            {
                //ARRANGE
                var command = new TestMessage(Guid.NewGuid(), Guid.NewGuid());
                var handlerInvoker = new HandlerInvoker();

                //ACT
                async Task Del()
                {
                    await handlerInvoker.InvokeHandlers(null, command, CancellationToken.None);
                }

                //ASSERT
                Assert.That(Del, Throws.ArgumentNullException);
            }

            [Test]
            public async Task
                Muliple_Exceptions_Thrown_Within_Handlers_The_Exceptions_Will_Bubble_Though_To_Invoker_As_An_Aggreated_Exception()
            {
                //ARRANGE
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                const string errorMessage = "Hello World! I am an error message";
                var command = new TestMessage(Guid.NewGuid(), Guid.NewGuid());
                var handler1 = new TestMessageHandler(() => throw new Exception(errorMessage), logger.Object, false);
                var handler2 = new TestMessageHandler(() => throw new Exception(errorMessage), logger.Object, false);
                var handlerInvoker = new HandlerInvoker();

                //ACT
                try
                {
                    await handlerInvoker.InvokeHandlers(new object[] {handler1, handler2}, command, CancellationToken.None);
                }
                catch (AggregateException ae)
                {
                    //ASSERT
                    Assert.That(ae.InnerExceptions.Count, Is.EqualTo(2));
                }
            }

            [Test]
            public async Task Multiple_Handlers_Can_Be_Invoked()
            {
                //ARRANGE
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                var result1 = 0;
                var result2 = 0;
                var command = new TestMessage(Guid.NewGuid(), Guid.NewGuid());
                var handler1 = new TestMessageHandler(() => result1 = 10, logger.Object, false);
                var handler2 = new TestMessageHandler(() => result2 = 20, logger.Object, false);
                var handlerInvoker = new HandlerInvoker();
                //ACT
                await handlerInvoker.InvokeHandlers(new object[] {handler1, handler2}, command, CancellationToken.None);

                //ASSERT
                Assert.That(result1, Is.EqualTo(10));
                Assert.That(result2, Is.EqualTo(20));
            }

            [Test]
            public async Task Single_Handler_Can_Be_Invoked()
            {
                //ARRANGE
                var result = 0;
                var logger = new Mock<IGWLogger<BaseMessageHandler<TestMessage>>>();
                var command = new TestMessage(Guid.NewGuid(), Guid.NewGuid());
                var handler = new TestMessageHandler(() => result = 10, logger.Object, false);
                var handlerInvoker = new HandlerInvoker();
                //ACT
                await handlerInvoker.InvokeHandlers(new[] {handler}, command, CancellationToken.None);

                //ASSERT
                Assert.That(result, Is.EqualTo(10));
            }
        }
    }
}