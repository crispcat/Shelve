namespace Shelve.Core
{
    using System;

    [Serializable]
    public class Variable : Sequence, IAffectable
    {
        private bool preventSelfInvoke;

        public void Affect() => Reset();

        public bool IsAffected { get; private set; }

        public override void Reset()
        {
            IsAffected = true;
            preventSelfInvoke = false;
        }

        public Variable(string name, Number initialValue) : base(name, initialValue) => Reset();

        public override Number Calculate()
        {
            if (/*!IsAffected &&*/ preventSelfInvoke)
            {
                return LastValue;
            }

            LastValue = InitialValue;

            //IsAffected = false;

            preventSelfInvoke = true;

            var result = base.Calculate();

            preventSelfInvoke = false;

            return result;
        }
    }
}
