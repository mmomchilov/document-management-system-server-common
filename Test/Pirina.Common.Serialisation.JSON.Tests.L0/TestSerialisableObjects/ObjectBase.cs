using System;

namespace Glasswall.Common.Serialisation.JSON.Tests.L0.SerialisableObjects
{
    [Serializable]
    public abstract class ObjectBase
    {
        public int Number => 10;
        public string Words => "Some Words";
    }
}
