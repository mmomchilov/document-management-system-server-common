using System;
using System.Collections.Generic;

namespace Glasswall.Common.Serialisation.Binary.Tests.L0.TestSerialisableObjects
{
    [Serializable]
    public class ComplexTypeSerialisableObject : ObjectBase
    {
        public Dictionary<int, string> Dictionary => new Dictionary<int, string> { {1, "One"}, {2, "Two"}, {3, "Three"} };
        public List<int> List => new List<int> { 0, 1, 1, 2, 3, 5, 8 };
        public IEnumerable<int> Enumerable => new [] { 0, 1, 1, 2, 3, 5, 8 };
        public Exception Exception => new Exception("This is an exception!");
    }
}
