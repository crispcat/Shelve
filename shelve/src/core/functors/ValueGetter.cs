namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal sealed class ValueGetter : IFunctor
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
            TargetSource = targetSource;
            ParamsCount = 0;
            Inner = null;
        }

        public IValueHolder Calculate()
        {
            if (!TargetSource.Contains(Name))
            {
                throw new Exception($"Variable {Name} does not exist in current context. " +
                    $"If the set is dependent merge it with main set or declare variable {Name}.");
            }

            return TargetSource[Name];
        }

        public IFunctor SetInnerArgs(IEnumerable<IValueHolder> args) => this;
    }
}
