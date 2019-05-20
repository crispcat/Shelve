namespace Shelve.Core
{
    using System;

    internal class LastValueGetter : ValueGetter
    {
        public LastValueGetter(string name, HashedVariables target) : base(name, target) { }

        public override IValueHolder Calculate()
        {
            if (!TargetSource.Contains(Name))
            {
                throw new Exception($"Variable {Name} does not exist in current context. " +
                    $"If the set is dependent, merge it with another set or declare a variable {Name}.");
            }

            return new ValueHolder(TargetSource[Name].LastValue);
        }
    }
}

