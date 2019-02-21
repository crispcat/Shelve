using Priority_Queue;
using System.Numerics;

namespace Shelve.Core
{
    public sealed class Expression : FastPriorityQueueNode
    {
        private Variable target;
        private Variable[] depends;

        public readonly string Name;

        public Polynumber Calculate()
        {
            throw new System.Exception();
        }
    }
}
