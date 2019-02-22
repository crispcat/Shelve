using System.Numerics;

namespace Shelve.Core
{
    public sealed class Expression
    {
        private short priority;

        private Variable target;
        private Variable[] depends;

        public readonly string Name;

        public double Calculate()
        {
            throw new System.NotImplementedException();
        }
    }
}
