namespace Shelve.Core
{
    using System;

    [Serializable]
    public sealed class StaticVariable : IValueHolder, IAffector
    {
        public Number Value { get; private set; }

        public Number LastValue => Value;

        public StaticVariable(Number number) => Set(number);

        public void Set(Number number)
        {
            Value = number;
        }
    }
}
