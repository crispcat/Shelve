namespace Shelve.Core
{
    using System;

    public class HashedNode<T> : IEquatable<HashedNode<T>> where T : IEquatable<T>
    {
        public readonly T Value;
        public readonly int Priority;

        public int ID => GetHashCode();

        public HashedNode(T value, int priority)
        {
            Value = value;
            Priority = priority;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Priority;
        }

        public bool Equals(HashedNode<T> other)
        {
            return other != null && Priority == other.Priority && Value.Equals(other.Value);
        }
    }
}