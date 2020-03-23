using System;
using System.Collections.Generic;
using System.Text;

namespace Glasswall.Common.CQRS.Tests.L0.TestMocks
{
    public class TestModel
    {
        public int Id { get; }

        public TestModel(int id)
        {
            Id = id;
        }
    }
}
