namespace Shelve.Tests
{
    using Shelve.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal static class SequenceTester
    {
        public static bool IsEqual(LinkedList<Lexema> target, Queue<Lexema> sample)
        {
            if (target.Count != sample.Count)
            {
                return false;
            }

            for (int i = 0; i < target.Count; i++)
            {
                if (target.DequeueHead() != sample.Dequeue())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
