namespace Shelve.Core
{
    using System.Collections.Generic;

    public enum MemberType
    {
        Operand, Operation
    }

    public enum Associativity
    {
        Left, Right
    }

    internal interface IFunctor
    {
        string Name { get; }
        MemberType Type { get; }
        int ParamsCount { get; }
        IValueHolder[] Inner { get; }

        int Priority { get; }
        Associativity Order { get; }

        IValueHolder Calculate();
        IFunctor SetInnerArgs(IEnumerable<IValueHolder> args);
    }
}
