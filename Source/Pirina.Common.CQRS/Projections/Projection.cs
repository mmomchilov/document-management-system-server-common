using System;

namespace Glasswall.Common.CQRS.Projections
{
    public class Projection : IEquatable<Projection>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public bool Equals(Projection other)
        {
            if (other == null)
                return false;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Projection projectionObj = obj as Projection;
            return projectionObj != null && Equals(projectionObj);
        }

        public static bool operator ==(Projection projection1, Projection projection2)
        {
            if (((object)projection1) == null || ((object)projection2) == null)
                return Object.Equals(projection1, projection2);

            return projection1.Equals(projection2);
        }

        public static bool operator !=(Projection projection1, Projection projection2)
        {
            if (((object)projection1) == null || ((object)projection2) == null)
                return !Object.Equals(projection1, projection2);

            return !(projection1.Equals(projection2));
        }
    }
}