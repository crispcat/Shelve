namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    public sealed class Expression
    {
        public readonly short priority;

        public readonly string Name;

        public Number Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
