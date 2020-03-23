using System;
using NUnit.Framework;

namespace Glasswall.Common.Configuration.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Configuration.Tests.L0")]
    public class ConfigurationExtensionTests
    {
        class ValidTestConfigurationKeyStore
        {
            public const string FirstKey = "FirstKey";
            public const string SecondKey = "SecondKey";
        }

        class InvalidTestConfigurationKeyStore
        {
            public const int InvalidKey = 0;
        }

        [Test]
        public void Missing_Configuration_Key_Results_In_ArgumentException()
        {
            // Arrange
            var configuration = new LocalStoreConfiguration
            {
                ["FirstKey"] = "FirstKey"
            };
            // Act
            void TestDelegate()
            {
                configuration.ValidateConfigurationContent(typeof(ValidTestConfigurationKeyStore));
            }

            // Assert
            Assert.That(TestDelegate,
                Throws.Exception
                    .TypeOf<ArgumentException>()
                    .With.Message.Contains("Missing configuration for SecondKey"));
        }

        [Test]
        public void Non_String_Configuration_Key_Results_In_ArgumentException()
        {
            // Arrange
            var configuration = new LocalStoreConfiguration
            {
                ["FirstKey"] = "FirstKey"
            };

            // Act
            void TestDelegate()
            {
                configuration.ValidateConfigurationContent(typeof(InvalidTestConfigurationKeyStore));
            }

            // Assert
            Assert.That(TestDelegate,
                Throws.Exception
                    .TypeOf<ArgumentException>()
                    .With.Message.Contains("Configuration contains invalid data types"));
        }

        [Test]
        public void Additional_Configuration_Keys_Does_Not_Fail_Validation()
        {
            // Arrange
            var configuration = new LocalStoreConfiguration
            {
                ["FirstKey"] = "FirstKey",
                ["SecondKey"] = "SecondKey",
                ["ExtraKey"] = "ExtraKey"
            };

            // Act
            void TestDelegate()
            {
                configuration.ValidateConfigurationContent(typeof(ValidTestConfigurationKeyStore));
            }

            // Assert
            Assert.That(TestDelegate, Throws.Nothing);
        }

        [Test]
        public void Correct_Configuration_Keys_Does_Not_Fail_Validation()
        {
            // Arrange
            var configuration = new LocalStoreConfiguration
            {
                ["FirstKey"] = "FirstKey",
                ["SecondKey"] = "SecondKey",
            };

            // Act
            void TestDelegate()
            {
                configuration.ValidateConfigurationContent(typeof(ValidTestConfigurationKeyStore));
            }

            // Assert
            Assert.That(TestDelegate, Throws.Nothing);
        }
    }
}
