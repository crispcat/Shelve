namespace Shelve.Core
{
    using System;

    [Serializable]
    public class Iterator : Sequence
    {
        private bool mustCalculateNextValue;

        public void MoveNextValue() => mustCalculateNextValue = true;

        public Iterator(string name, Number value) : base(name, value)
        {
            mustCalculateNextValue = false;
        }

        public override Number Calculate()
        {
            if (mustCalculateNextValue)
            {
                return LastValue;
            }

            mustCalculateNextValue = false;

            return base.Calculate();
        }

        public override void Reset()
        {
            LastValue = InitialValue;
            mustCalculateNextValue = false;
        }
    }
}
