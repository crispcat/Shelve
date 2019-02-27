namespace Shelve.Core
{
    using System;

    [Serializable]
    public class Variable : Sequence, IAffectable
    {
        public void Affect() => Reset();

        public bool IsAffected { get; private set; }

        public override void Reset() => IsAffected = true;

        public Variable(string name, Number initialValue) : base(name, initialValue) => Reset();

        public override Number Calculate()
        {
            if (!IsAffected)
            {
                return LastValue;
            }

            LastValue = InitialValue;
            IsAffected = false;

            return base.Calculate();
        }
    }
}
