namespace Shelve.Core
{
    public interface IAffector
    {
        void AffectAllDependencies();
        void AddAffectOn(IAffectable dependency);
    }
}