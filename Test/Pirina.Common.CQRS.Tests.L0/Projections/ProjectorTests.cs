using System;
using System.Collections.Generic;
using System.Linq;
using Glasswall.Common.CQRS.Projections;
using Glasswall.Common.CQRS.Tests.L0.TestMocks;
using Glasswall.Kernel.Data.ORM;
using Moq;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0.Projections
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class ProjectorTests
    {
        [TestFixture]
        public class Constructor : ProjectorTests
        {
            [Test]
            public void Null_Db_Context_Throws_Argument_Null_Exception()
            {
                //Arrange

                //Act
                Projector<TestModel> TestDelegate() => new Projector<TestModel>(null);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("context"));
            }

            [Test]
            public void Can_Be_Successfully_Constructed()
            {
                //Arrange

                //Act
                Projector<TestModel> TestDelegate() => new Projector<TestModel>(Mock.Of<IDbContext>());

                //Assert
                Assert.That(TestDelegate, Throws.Nothing);
            }
        }

        [TestFixture]
        public class GetAllMethod : ProjectorTests
        {
            [Test]
            public void Returns_Queryable_Of_Type()
            {
                //Arrange
                var expectedCollection = new List<TestModel>
                {
                    new TestModel(0),
                    new TestModel(1)
                };
                var context = new Mock<IDbContext>();
                context.Setup(x => x.Set<TestModel>()).Returns(expectedCollection.AsQueryable());
                var projector = new Projector<TestModel>(context.Object);

                //Act
                var actualCollection = projector.GetAll().ToList();

                //Assert
                Assert.That(actualCollection, Has.Count.EqualTo(expectedCollection.Count));
                Assert.That(actualCollection.First().Id, Is.EqualTo(0));
            }

            [Test]
            public void Failure_Getting_Set_Will_Bubble_Through()
            {
                //Arrange
                const string exceptionMessage = "Test Exception";
                var context = new Mock<IDbContext>();
                context.Setup(x => x.Set<TestModel>()).Throws(new Exception(exceptionMessage));
                var projector = new Projector<TestModel>(context.Object);

                //Act
                IQueryable<TestModel> TestDelegate() => projector.GetAll();

                //Assert
                Assert.That(TestDelegate, Throws.Exception.With.Message.EqualTo(exceptionMessage));
            }

            [Test]
            public void Can_Return_Empty_Queryable()
            {
                //Arrange
                var context = new Mock<IDbContext>();
                context.Setup(x => x.Set<TestModel>()).Returns(new List<TestModel>().AsQueryable);
                var projector = new Projector<TestModel>(context.Object);

                //Act
                var result = projector.GetAll().ToList();

                //Assert
                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class DisposeMethod : ProjectorTests
        {
            [Test]
            public void Will_Dispose_Of_Db_Context()
            {
                //Arrange
                var context = new Mock<IDbContext>();
                var projector = new Projector<TestModel>(context.Object);

                //Act
                projector.Dispose();

                //Assert
                context.Verify(x => x.Dispose(), Times.Once);
            }
        }
    }
}
