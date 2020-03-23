using Glasswall.Common.CQRS.Projections;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0.Projections
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class ProjectionTests
    {
        private const int Id = 10;
        private const string Name = "Barry Von Barriton";
        private const string DisplayName = "Barry";

        [TestFixture]
        public class IdProperty : ProjectionTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var projection = new Projection {Id = Id };

                //Act
                var actualId = projection.Id;

                //Assert
                Assert.AreEqual(Id, actualId);
            }

            [Test]
            public void By_Default_Is_0()
            {
                //Arrange
                var projection = new Projection();

                //Act
                var actualId = projection.Id;

                //Assert
                Assert.AreEqual(0, actualId);
            }
        }

        [TestFixture]
        public class NameProperty : ProjectionTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var projection = new Projection { Name = Name };

                //Act
                var actualName = projection.Name;

                //Assert
                Assert.AreEqual(Name, actualName);
            }

            [Test]
            public void By_Default_Is_Null()
            {
                //Arrange
                var projection = new Projection();

                //Act
                var actualName = projection.Name;

                //Assert
                Assert.That(actualName, Is.Null);
            }
        }

        [TestFixture]
        public class DisplayNameProperty : ProjectionTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var projection = new Projection { DisplayName = DisplayName };

                //Act
                var actualName = projection.DisplayName;

                //Assert
                Assert.AreEqual(DisplayName, actualName);
            }

            [Test]
            public void By_Default_Is_Null()
            {
                //Arrange
                var projection = new Projection();

                //Act
                var actualName = projection.DisplayName;

                //Assert
                Assert.That(actualName, Is.Null);
            }
        }

        [TestFixture]
        public class EqualsMethod : ProjectionTests
        {
            [Test]
            public void Returns_True_If_Ids_Match()
            {
                //Arrange
                var projection = new Projection {Id = Id};
                var otherProjection = new Projection {Id = 10};

                //Act
                var result = projection.Equals(otherProjection);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Returns_False_If_Other_Projection_Is_Null()
            {
                //Arrange
                var projection = new Projection { Id = Id };

                //Act
                var result = projection.Equals(null);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void Returns_True_Even_If_Names_Differ()
            {
                //Arrange
                var projection = new Projection { Id = Id, Name = Name};
                var otherProjection = new Projection { Id = 10, Name = "My Name Doesn't Matter"};

                //Act
                var result = projection.Equals(otherProjection);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Returns_True_Even_If_Display_Names_Differ()
            {
                //Arrange
                var projection = new Projection { Id = Id, DisplayName = DisplayName};
                var otherProjection = new Projection { Id = 10, DisplayName = "My Display Name Doesn't Matter" };

                //Act
                var result = projection.Equals(otherProjection);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Returns_False_If_Null_Object()
            {
                //Arrange
                var projection = new Projection { Id = Id, DisplayName = DisplayName };
                object other = null;

                //Act
                var result = projection.Equals(other);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void Returns_False_If_Object_Cannot_Be_Cast_To_Projection()
            {
                //Arrange
                var projection = new Projection { Id = Id, DisplayName = DisplayName };
                object other = 10;

                //Act
                var result = projection.Equals(other);

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void Returns_True_If_Object_Match_After_Cast_From_Object()
            {
                //Arrange
                var projection = new Projection { Id = Id };
                object otherProjection = new Projection { Id = 10 };

                //Act
                var result = projection.Equals(otherProjection);

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Can_Be_Positively_Compaired_With_Operator_And_Return_True()
            {
                //Arrange
                var projection = new Projection { Id = Id };
                var otherProjection = new Projection { Id = 10 };

                //Act
                var result = projection == otherProjection;

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Can_Be_Positively_Compaired_With_Operator_And_Return_False()
            {
                //Arrange
                var projection = new Projection { Id = Id };
                var otherProjection = new Projection { Id = 11 };

                //Act
                var result = projection == otherProjection;

                //Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void Can_Be_Negatively_Compaired_With_Operator_And_Return_True()
            {
                //Arrange
                var projection = new Projection { Id = Id };
                var otherProjection = new Projection { Id = 11 };

                //Act
                var result = projection != otherProjection;

                //Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void Can_Be_Negatively_Compaired_With_Operator_And_Return_False()
            {
                //Arrange
                var projection = new Projection { Id = Id };
                var otherProjection = new Projection { Id = 10 };

                //Act
                var result = projection != otherProjection;

                //Assert
                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class GetHashCodeMethod : ProjectionTests
        {
            [Test]
            public void Hash_Of_Int_Equal_To_Id_Should_Be_The_Same()
            {
                //Arrange
                var expectedHash = Id.GetHashCode();
                var projection = new Projection {Id = Id};

                //Act
                var actualHash = projection.GetHashCode();

                //Assert
                Assert.AreEqual(expectedHash, actualHash);
            }
        }
    }
}
