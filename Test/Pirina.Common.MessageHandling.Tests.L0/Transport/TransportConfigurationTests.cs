using System;
using Glasswall.Common.Transport;
using Glasswall.Kernel.Transport;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.MessageHandling.Tests.L0.Transport
{
    [TestFixture]
    [Category("Glasswall.Common.MessageHandling.Tests.L0")]
    public class TransportConfigurationTests
    {
        [TestFixture]
        public class Constructor : TransportConfigurationTests
        {
            [Test]
            public void The_Default_Constructor_Will_Initialise_An_Emtpy_List_Of_Listeners()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.Listeners, Is.Not.Null);
            }

            [Test]
            public void The_Default_Value_For_Max_Degree_Of_Parallelism_Is_0()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.MaxDegreeOfParallelism, Is.EqualTo(0));
            }

            [Test]
            public void The_Default_Value_For_Concurrent_Listeners_Is_0()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.ConcurrentListeners, Is.EqualTo(0));
            }

            [Test]
            public void The_Default_Value_For_Consumer_Period_Is_0_Seconds()
            {
                //Arrange
                var consumerPeriod = default(TimeSpan);

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.ConsumerPeriod, Is.EqualTo(consumerPeriod));
            }

            [Test]
            public void The_Default_Value_For_Transport_Connection_Is_Null()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.TransportConnection, Is.Null);
            }

            [Test]
            public void The_Default_Value_For_Is_Transactional_Is_False()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.IsTransactional, Is.False);
            }

            [Test]
            public void The_Default_Value_For_Transport_Mode_Is_NotInitialised()
            {
                //Arrange

                //Act
                var config = new TransportConfiguration();

                //Assert
                Assert.That(config.TransportMode, Is.EqualTo(Mode.NotInitialised));
            }
        }

        [TestFixture]
        public class MaxDegreeOfParallelismProperty : TransportConfigurationTests
        {
            [Test]
            public void The_Max_Degree_Of_Parallelism_Can_Be_Set_At_Runtime()
            {
                //Arrange
                const int expectedMaxDegreeOfParallelism = 5;
                var config = new TransportConfiguration();

                //Act
                config.MaxDegreeOfParallelism = expectedMaxDegreeOfParallelism;

                //Assert
                Assert.That(config.MaxDegreeOfParallelism, Is.EqualTo(expectedMaxDegreeOfParallelism));
            }
        }

        [TestFixture]
        public class ConcurrentListenersProperty : TransportConfigurationTests
        {
            [Test]
            public void The_Concurrent_Listeners_Can_Be_Set_At_Runtime()
            {
                //Arrange
                const int expectedConcurrentListeners = 5;
                var config = new TransportConfiguration();

                //Act
                config.ConcurrentListeners = expectedConcurrentListeners;

                //Assert
                Assert.That(config.ConcurrentListeners, Is.EqualTo(expectedConcurrentListeners));
            }
        }

        [TestFixture]
        public class ConsumerPeriodProperty : TransportConfigurationTests
        {
            [Test]
            public void The_Consumer_Period_Can_Be_Set_At_Runtime()
            {
                //Arrange
                var expectedConsumerPeriod = TimeSpan.FromSeconds(5);
                var config = new TransportConfiguration();

                //Act
                config.ConsumerPeriod = expectedConsumerPeriod;

                //Assert
                Assert.That(config.ConsumerPeriod, Is.EqualTo(expectedConsumerPeriod));
            }
        }

        [TestFixture]
        public class ListenersPropertry : TransportConfigurationTests
        {
            [Test]
            public void Listeners_Can_Be_Added_To_At_Runtime()
            {
                //Arrange
                var mockListener = new Mock<IMessageListener>();
                var config = new TransportConfiguration();

                //Act
                config.Listeners.Add(mockListener.Object);

                //Assert
                Assert.That(config.Listeners, Has.Count.EqualTo(1));
            }
        }

        [TestFixture]
        public class TransportConnectionProperty : TransportConfigurationTests
        {
            [Test]
            public void The_Transport_Connection_Can_Be_Set_At_Runtime()
            {
                //Arrange
                const string connectionString = "Foo";
                const string queueName = "Bar";
                var transportConnection = new TransportConnection(connectionString, queueName);
                var config = new TransportConfiguration();

                //Act
                config.TransportConnection = transportConnection;

                //Assert
                Assert.That(config.TransportConnection, Is.Not.Null);
                Assert.That(config.TransportConnection.ConnectionString, Is.EqualTo(connectionString));
                Assert.That(config.TransportConnection.QueueName, Is.EqualTo(queueName));
            }
        }

        [TestFixture]
        public class IsTransactional : TransportConfigurationTests
        {
            [Test]
            public void The_Is_Transactional_Can_Be_Set_At_Runtime()
            {
                //Arrange
                var expectedIsTransactional = true;
                var config = new TransportConfiguration();

                //Act
                config.IsTransactional = expectedIsTransactional;

                //Assert
                Assert.That(config.IsTransactional, Is.True);
            }
        }

        [TestFixture]
        public class TransportMode : TransportConfigurationTests
        {
            [Test]
            public void The_Transport_Mode_Can_Be_Set_At_Runtime()
            {
                //Arrange
                var expectedTransportMode = Mode.ReceiveOnly;
                var config = new TransportConfiguration();

                //Act
                config.TransportMode = expectedTransportMode;

                //Assert
                Assert.That(config.TransportMode, Is.EqualTo(Mode.ReceiveOnly));
            }
        }
    }
}
