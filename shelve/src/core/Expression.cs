namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    public sealed class Expression : IEquatable<Expression>
    {
        public readonly short priority;

        private Variable target;
        private HashSet<Variable> depends;

        public readonly string Name;

        public double Calculate(IParallelAccess target)
        {
            throw new System.NotImplementedException();
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
