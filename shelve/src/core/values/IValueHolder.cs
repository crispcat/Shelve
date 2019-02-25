namespace Shelve.Core
{
    public interface IValueHolder
    {
        Number Value { get; }
        Number LastValue { get; }
    }
}
