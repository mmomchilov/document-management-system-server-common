using System;

namespace Glasswall.Common.Serialisation.Binary.Tests.L0.TestSerialisableObjects
{
    [Serializable]
    public abstract class ObjectBase
    {
        public int Number => 10;
        public string Words => "Some Words";
    }
}
