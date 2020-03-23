using System;
using System.Threading.Tasks;
using Glasswall.Common.Serialisation.JSON.Tests.L0.SerialisableObjects;
using Glasswall.Common.Serialisation.JSON.SettingsProviders;
using Glasswall.Kernel.Serialisation;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Glasswall.Common.Serialisation.JSON.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Serialisation.JSON.Tests.L0")]
    public class NSJsonSerialiserTests
    {
        [TestFixture]
        public class Constructor : NSJsonSerialiserTests
        {
            [Test]
            public void Binary_Serialiser_Can_Be_Constructed_Using_its_Constructor()
            {
                //Arrange

                //Act
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings);

                //Assert
                Assert.That(serialiser, Is.Not.Null);
            }

            [Test]
            public void Binary_Serialiser_null_settings()
            {
                //Arrange

                //Act

                //Assert
                Assert.Throws<ArgumentNullException>(() => new NSJsonSerializer(null));
            }
        }

        [TestFixture]
        public class SerialiseMethod : NSJsonSerialiserTests
        {
            [Test]
            public async Task Successful_Serialisation_Will_Return_A_Stream_Of_The_Serialised_Object()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;

                //Act
                var result = await serialiser.Serialise(obj);

                //Assert
                Assert.That(result, Is.Not.Null);
            }

            [Test]
            public async Task Providing_Null_Will_Result_In_null()
            {
                //Arrange
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;

                //Act
                var result = await serialiser.Serialise(null);

                //Assert
                Assert.IsNotNull(result);
            }
            
            [Test]
            public async Task Objects_Containing_More_Complex_Types_Can_Be_Serialised()
            {
                //Arrange
                var obj = new ComplexTypeSerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;

                //Act
                var result = await serialiser.Serialise(obj);

                //Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        [TestFixture]
        public class DeserialiseMethod : NSJsonSerialiserTests
        {
            [Test]
            public async Task Successful_Deserialisation_Will_Return_An_Object()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<SerialisableObject>(stream);

                //Assert
                Assert.That(result.Number, Is.EqualTo(obj.Number));
                Assert.That(result.Words, Is.EqualTo(obj.Words));
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }

            [Test]
            public void Providing_Null_Will_Result_In_An_Argument_Null_Exception()
            {
                //Arrange
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;

                //Act
                async Task Delegate()
                {
                    await serialiser.Deserialise(null);
                }

                //Assert
                Assert.That(Delegate, Throws.ArgumentNullException);
            }
            
            [Test]
            public async Task Object_Containing_More_Complex_Types_Can_Be_Deserialised()
            {
                //Arrange
                var obj = new ComplexTypeSerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = (ComplexTypeSerialisableObject)await serialiser.Deserialise<ComplexTypeSerialisableObject>(stream);

                //Assert
                Assert.That(result.Exception.Message, Is.EqualTo(obj.Exception.Message));
                Assert.That(result.Dictionary, Is.EqualTo(obj.Dictionary));
                Assert.That(result.Enumerable, Is.EqualTo(obj.Enumerable));
                Assert.That(result.List, Is.EqualTo(obj.List));
                Assert.That(result, Is.TypeOf<ComplexTypeSerialisableObject>());
            }

            [Test]
            public async Task An_Object_That_Exists_In_Two_Different_Namespaces_Can_Be_Deserialised_To_The_Correct_Type()
            {
                //Arrange
                var obj = new NameSpaceSerialisationObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<NameSpaceSerialisationObject>(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<NameSpaceSerialisationObject>());
            }
        }

        [TestFixture]
        public class JsonDeserialiseMethod : NSJsonSerialiserTests
        {
            [Test]
            public async Task Successful_Deserialisation_Will_Return_An_Object_Of_Type_Specified()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<SerialisableObject>(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }
            
            [Test]
            public async Task Providing_A_Base_Class_Will_Cast_The_Return_The_Object_Cast_To_The_Base_Class()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings) as ISerialiser;
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<SerialisableObject>(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }
        }

        [TestFixture]
        public class GenericDeserialiseMethod : NSJsonSerialiserTests
        {
            [Test]
            public async Task Successful_Deserialisation_Will_Return_An_Object_Of_Type_Specified()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings);
                var serialised = await serialiser.SerialiseToJson(obj);
               
                //Act
                var result = await serialiser.DeserialiseFromJson<SerialisableObject>(serialised);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }

            [Test]
            public async Task DeserialisingToJsonObject()
            {
                //Arrange
                var obj = new SerialisableObject();
                var settings = new DefaultSettingsProvider();
                var serialiser = new NSJsonSerializer(settings);
                var serialised = await serialiser.SerialiseToJson(obj);
                
                //Act
                var result = await serialiser.DeserialiseFromJson(serialised);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<JObject>());
            }
        }
    }
}