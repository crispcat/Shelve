namespace Shelve.Core
{
    internal interface ISwichableMatcher
    {
        bool IsActive { get; set; }
        int Match(string text);
    }
}
