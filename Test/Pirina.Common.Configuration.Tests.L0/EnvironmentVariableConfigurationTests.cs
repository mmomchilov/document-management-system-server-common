using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Glasswall.Common.Configuration.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Configuration.Tests.L0")]
    public class EnvironmentVariableConfigurationTests
    {
        private const string TestKey = "TestKey";
        private const string TestValue = "Test Value";
        private const string UpdatedValue = "Updated Value";

        [TestFixture]
        public class GetConfigurationValue : EnvironmentVariableConfigurationTests
        {
            [Test]
            //[Ignore("setting of evironment variables requires admin privileges, this may not be suitable for a build environment")]
            public void Specified_Configuration_Value_Can_Be_Retrieved()
            {
                // Arrange
                Environment.SetEnvironmentVariable(TestKey, TestValue);

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var resultValue = testConfiguration[TestKey];

                // Assert
                Assert.That(resultValue, Is.EqualTo(TestValue));
            }

            [Test]
            public void Undefined_Configuration_Value_Throws_A_KeyNotFoundException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    var result = testConfiguration["MissingKey"];
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.TypeOf<KeyNotFoundException>()
                        .With.Message.EqualTo("A configuration item with the key 'MissingKey' was not found"));
            }

            [Test]
            public void Null_Configuration_Key_Throws_ArgumentNullException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    var unused = testConfiguration[null];
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.Exception
                        .TypeOf<ArgumentNullException>()
                        .With.Property("ParamName")
                        .EqualTo("variable"));
            }
        }

        [TestFixture]
        public class SetConfigurationValue : EnvironmentVariableConfigurationTests
        {
            [Test]
            //[Ignore("setting of evironment variables requires admin privileges, this may not be suitable for a build environment")]
            public void Specified_Configuration_Value_Is_Stored()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration[TestKey] = TestValue;

                // Assert
                var result = Environment.GetEnvironmentVariable(TestKey);
                Assert.That(result, Is.EqualTo(TestValue));
            }

            [Test]
            public void Null_Configuration_Key_Throws_ArgumentNullException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    testConfiguration[null] = string.Empty;
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.Exception
                        .TypeOf<ArgumentNullException>()
                        .With.Property("ParamName")
                        .EqualTo("variable"));
            }

            [Test]
            public void Setting_Existing_Value_Updates_Store_To_New_Value()
            {
                var testConfiguration = new EnvironmentVariableConfiguration()
                {
                    [TestKey] = TestValue
                };

                // Act
                testConfiguration[TestKey] = UpdatedValue;

                // Assert
                Assert.That(testConfiguration[TestKey], Is.EqualTo(UpdatedValue));
            }
        }

        [TestFixture]
        public class GetValueOfTypeMethod : EnvironmentVariableConfigurationTests
        {
            [Test]
            public void Can_Return_Value_As_String()
            {
                // Arrange
                Environment.SetEnvironmentVariable(TestKey, TestValue);

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<string>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(TestValue));
            }

            [Test]
            public void Can_Return_Value_As_Char()
            {
                // Arrange
                const char expectedTestValue = 'A';
                Environment.SetEnvironmentVariable(TestKey, expectedTestValue.ToString());

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<char>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Can_Return_Value_As_Int()
            {
                // Arrange
                const int expectedTestValue = 1000;
                Environment.SetEnvironmentVariable(TestKey, expectedTestValue.ToString());

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<int>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Can_Return_Value_As_Short()
            {
                // Arrange
                const short expectedTestValue = short.MaxValue;
                Environment.SetEnvironmentVariable(TestKey, Convert.ToString(expectedTestValue, CultureInfo.InvariantCulture));

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<short>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Can_Return_Value_As_Double()
            {
                // Arrange
                const double expectedTestValue = 1234.5678;
                Environment.SetEnvironmentVariable(TestKey, Convert.ToString(expectedTestValue, CultureInfo.InvariantCulture));

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<double>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Can_Return_Value_As_Long()
            {
                // Arrange
                const long expectedTestValue = long.MaxValue;
                Environment.SetEnvironmentVariable(TestKey, expectedTestValue.ToString());

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<long>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Can_Return_Value_As_Bool()
            {
                // Arrange
                const bool expectedTestValue = true;
                Environment.SetEnvironmentVariable(TestKey, expectedTestValue.ToString());

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                var result = testConfiguration.GetValue<bool>(TestKey);

                // Assert
                Assert.That(result, Is.EqualTo(expectedTestValue));
            }

            [Test]
            public void Attemtping_To_Get_A_Complex_Type_Throws_TypeAccessException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    testConfiguration.GetValue<DateTime>(TestKey);
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.TypeOf<TypeAccessException>()
                        .With.Message.EqualTo("Only strings and primitive types can be stored in configuration"));
            }

            [Test]
            public void Passing_A_Null_Key_Throws_An_ArgumentNullException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    testConfiguration.GetValue<int>(null);
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.ArgumentNullException
                        .With.Property("ParamName").EqualTo("variable"));
            }

            [Test]
            public void Passing_A_Non_Existant_Key_Throws_A_KeyNotFoundException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    var result = testConfiguration.GetValue<int>("MissingKey");
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.TypeOf<KeyNotFoundException>()
                        .With.Message.EqualTo("A configuration item with the key 'MissingKey' was not found"));  
            }
        }

        [TestFixture]
        public class SetValueOfTypeMethod : EnvironmentVariableConfigurationTests
        {
            [Test]
            public void Can_Set_String_Value()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<string>(TestKey, TestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(TestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Char_Value()
            {
                // Arrange
                const char expectedTestValue = 'A';

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<char>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Int_Value()
            {
                // Arrange
                const int expectedTestValue = 1000;
                Environment.SetEnvironmentVariable(TestKey, expectedTestValue.ToString());

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<int>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Short_Value()
            {
                // Arrange
                const short expectedTestValue = short.MaxValue;

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
               testConfiguration.SetValue<short>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Double_Value()
            {
                // Arrange
                const double expectedTestValue = 1234.5678;

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<double>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Long_Value()
            {
                // Arrange
                const long expectedTestValue = long.MaxValue;

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<long>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Can_Set_Bool_Value()
            {
                // Arrange
                const bool expectedTestValue = true;

                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                testConfiguration.SetValue<bool>(TestKey, expectedTestValue);

                // Assert
                Assert.That(Environment.GetEnvironmentVariable(TestKey), Is.EqualTo(expectedTestValue.ToString(CultureInfo.InvariantCulture)));
            }

            [Test]
            public void Attemtping_To_Set_A_Complex_Type_Throws_TypeAccessException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    testConfiguration.SetValue<DateTime>(TestKey, new DateTime(2018,01,01));
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.TypeOf<TypeAccessException>()
                        .With.Message.EqualTo("Only strings and primitive types can be stored in configuration"));
            }

            [Test]
            public void Passing_A_Null_Key_Throws_An_ArgumentNullException()
            {
                // Arrange
                var testConfiguration = new EnvironmentVariableConfiguration();

                // Act
                void TestDelegate()
                {
                    testConfiguration.SetValue<string>(null, TestValue);
                }

                // Assert
                Assert.That(TestDelegate,
                    Throws.ArgumentNullException
                        .With.Property("ParamName").EqualTo("variable"));
            }
        }
    }
}
