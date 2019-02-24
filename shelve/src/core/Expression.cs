namespace Shelve.Core
{
    using System;

    public sealed class Expression : IEquatable<Expression>
    {
        public readonly short priority;

        private Variable target;
        private Variable[] depends;

        public readonly string Name;

        public double Calculate(IParallelAccess sender)
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
