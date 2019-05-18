namespace Shelve.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Operator : IFunctor
    {
        public string Name { get; }

        public int Priority { get; }
        public Associativity Order { get; }

        public int ParamsCount { get; }
        public IValueHolder[] Inner { get; private set; }

        public MemberType Type => MemberType.Operation;

        private Func<IValueHolder[], IValueHolder> action;

        public Operator(string name, Func<IValueHolder[], IValueHolder> action, int paramsCount, 
            int priority, Associativity order)
        {
            Name = name;
            ParamsCount = paramsCount;
            Priority = priority;
            Order = order;

            this.action = action;
        }

        public IValueHolder Calculate() => action.Invoke(Inner);

        public IFunctor SetInnerArgs(IEnumerable<IValueHolder> args)
        {
            if (args.Count() != ParamsCount)
            {
                throw new InvalidOperationException($"Operator {Name} can take {ParamsCount} params." +
                    $"You passed {args.Count()}.");
            }

            Inner = args.ToArray();

            return this;
        }
    }
}
