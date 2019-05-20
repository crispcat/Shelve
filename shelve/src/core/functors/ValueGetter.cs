namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal class ValueGetter : IFunctor
    {
        public string Name { get; }

        public HashedVariables TargetSource;

        public int ParamsCount { get; }
        public IValueHolder[] Inner { get; private set; }

        public MemberType Type => MemberType.Operand;

        public int Priority => 4;

        public Associativity Order => Associativity.Left;

        public ValueGetter(string valueName, HashedVariables targetSource)
        {
            Name = valueName;
            TargetSource = targetSource;
            ParamsCount = 0;
            Inner = null;
        }

        public virtual IValueHolder Calculate()
        {
            if (!TargetSource.Contains(Name))
            {
                throw new Exception($"Variable {Name} does not exist in current context. " +
                    $"If the set is dependent, merge it with another set or declare a variable {Name}.");
            }

            return TargetSource[Name];
        }

        public IFunctor SetInnerArgs(IEnumerable<IValueHolder> args) => this;
    }
}
