using System;
using Glasswall.Kernel.Messaging;

namespace Glasswall.Common.MessageHandling.Tests.L0.TestMocks
{
    public class TestMessage : Message
    {
        public TestMessage(Guid id, Guid correlationId)
            : base(id)
        {
        }
    }
}