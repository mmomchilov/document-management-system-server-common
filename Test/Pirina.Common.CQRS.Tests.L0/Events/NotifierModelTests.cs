using System;
using Glasswall.Common.CQRS.Events;
using NUnit.Framework;

namespace Glasswall.Common.CQRS.Tests.L0.Events
{
    [TestFixture]
    [Category("Glasswall.Common.CQRS.Tests.L0")]
    public class NotifierModelTests
    {
        [TestFixture]
        public class TenantIdProperty : NotifierModelTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var expectedId = Guid.NewGuid();
                var model = new NotifierModel {TenantId = expectedId};

                //Act
                var actualId = model.TenantId;

                //Assert
                Assert.AreEqual(expectedId, actualId);
            }
        }

        [TestFixture]
        public class RequestIdProperty : NotifierModelTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var expectedId = Guid.NewGuid();
                var model = new NotifierModel { RequestId = expectedId };

                //Act
                var actualId = model.RequestId;

                //Assert
                Assert.AreEqual(expectedId, actualId);
            }
        }

        [TestFixture]
        public class SuccessProperty : NotifierModelTests
        {
            [Test]
            public void Can_Set_And_Get()
            {
                //Arrange
                var model = new NotifierModel { Success = true };

                //Act
                var actualBool = model.Success;

                //Assert
                Assert.AreEqual(true, actualBool);
            }

            [Test]
            public void By_Default_Success_Is_False()
            {
                //Arrange
                var model = new NotifierModel();

                //Act
                var actualBool = model.Success;

                //Assert
                Assert.AreEqual(false, actualBool);
            }
        }
    }
}
