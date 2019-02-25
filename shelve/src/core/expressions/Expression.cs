namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    public sealed class Expression : IEquatable<Expression>
    {
        public readonly short priority;

        private IValueHolder target;
        private HashSet<IValueHolder> depends;

        public readonly string Name;

        public Number Calculate()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int hashCode = Name.GetHashCode();

            foreach (var depend in depends)
            {
                hashCode ^= depend.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public bool Equals(Expression other)
        {
            return other != null && Equals(other);
        }
    }
}
