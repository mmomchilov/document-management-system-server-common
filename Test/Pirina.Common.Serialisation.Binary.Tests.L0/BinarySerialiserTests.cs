using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Glasswall.Common.Serialisation.Binary.Tests.L0.TestSerialisableObjects;
using NUnit.Framework;

namespace Glasswall.Common.Serialisation.Binary.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Serialisation.Binary.Tests.L0")]
    public class BinarySerialiserTests
    {
        [TestFixture]
        public class Constructor : BinarySerialiserTests
        {
            [Test]
            public void Binary_Serialiser_Can_Be_Constructed_Using_Default_Constructor()
            {
                //Arrange

                //Act
                var serialiser = new BinarySerialiser();

                //Assert
                Assert.That(serialiser, Is.Not.Null);
            }
        }

        [TestFixture]
        public class SerialiseMethod : BinarySerialiserTests
        {
            [Test]
            public async Task Successful_Serialisation_Will_Return_A_Stream_Of_The_Serialised_Object()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();

                //Act
                var result = await serialiser.Serialise(obj);

                //Assert
                Assert.That(result, Is.Not.Null);
            }

            [Test]
            public void Providing_Null_Will_Result_In_An_Argument_Null_Exception()
            {
                //Arrange
                var serialiser = new BinarySerialiser();

                //Act
                async Task Delegate() { await serialiser.Serialise(null); }

                //Assert
                Assert.That(Delegate, Throws.ArgumentNullException);
            }

            [Test]
            public void If_Object_Is_Not_Marked_As_Serialisable_Then_A_Serialisation_Exception_Will_Be_Thrown()
            {
                //Arrange
                var obj = new NotSerialisableObject();
                var serialiser = new BinarySerialiser();

                //Act
                async Task Delegate() { await serialiser.Serialise(obj); }

                //Assert
                Assert.That(Delegate, Throws.TypeOf<SerializationException>());
            }

            [Test]
            public async Task Objects_Containing_More_Complex_Types_Can_Be_Serialised()
            {
                //Arrange
                var obj = new ComplexTypeSerialisableObject();
                var serialiser = new BinarySerialiser();

                //Act
                var result = await serialiser.Serialise(obj);

                //Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        [TestFixture]
        public class DeserialiseMethod : BinarySerialiserTests
        {
            [Test]
            public async Task Successful_Deserialisation_Will_Return_An_Object()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = (SerialisableObject)await serialiser.Deserialise(stream);

                //Assert
                Assert.That(result.Number, Is.EqualTo(obj.Number));
                Assert.That(result.Words, Is.EqualTo(obj.Words));
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }

            [Test]
            public void Providing_Null_Will_Result_In_An_Argument_Null_Exception()
            {
                //Arrange
                var serialiser = new BinarySerialiser();

                //Act
                async Task Delegate()
                {
                    await serialiser.Deserialise(null);
                }

                //Assert
                Assert.That(Delegate, Throws.ArgumentNullException);
            }

            [Test]
            public async Task Providing_A_Stream_That_Is_Not_At_The_Beginning_Will_Result_In_A_Serialisation_Exception()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);

                //Act
                async Task Delegate()
                {
                    await serialiser.Deserialise(stream);
                }

                //Assert
                Assert.That(Delegate, Throws.TypeOf<SerializationException>());
            }

            [Test]
            public async Task Object_Containing_More_Complex_Types_Can_Be_Deserialised()
            {
                //Arrange
                var obj = new ComplexTypeSerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = (ComplexTypeSerialisableObject) await serialiser.Deserialise(stream);

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
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<NameSpaceSerialisationObject>());
            }
        }

        [TestFixture]
        public class GenericDeserialiseMethod : BinarySerialiserTests
        {
            [Test]
            public async Task Successful_Deserialisation_Will_Return_An_Object_Of_Type_Specified()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<SerialisableObject>(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }

            [Test]
            public async Task Providing_An_Type_Different_To_The_Deserialised_Type_Will_Result_In_An_Invalid_Cast_Exception()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                async Task Delegate()
                {
                    await serialiser.Deserialise<NotSerialisableObject>(stream);
                }

                //Assert
                Assert.That(Delegate, Throws.TypeOf<InvalidCastException>());
            }

            [Test]
            public async Task Providing_A_Base_Class_Will_Cast_The_Return_The_Object_Cast_To_The_Base_Class()
            {
                //Arrange
                var obj = new SerialisableObject();
                var serialiser = new BinarySerialiser();
                var stream = await serialiser.Serialise(obj);
                stream.Position = 0;

                //Act
                var result = await serialiser.Deserialise<ObjectBase>(stream);

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<SerialisableObject>());
            }
        }
    }
}
