namespace Shelve.Core
{
    public class HashedNode<T>
    {
        public readonly T Value;
        public readonly int Priority;

        public int ID => GetHashCode();

        public HashedNode(T value, int priority)
        {
            Value = value;
            Priority = priority;
        }
    }
}