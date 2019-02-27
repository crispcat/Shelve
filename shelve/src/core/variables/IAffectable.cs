namespace Shelve.Core
{
    public interface IAffectable
    {
        bool IsAffected { get; }
        void Affect();
    }
}