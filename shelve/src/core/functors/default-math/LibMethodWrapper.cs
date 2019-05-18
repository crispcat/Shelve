namespace Shelve.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    internal class LibMethodWrapper : IFunctor
    {
        public string Name => method.Name;

        public int ParamsCount { get; private set; }
        public IValueHolder[] Inner { get; private set; }

        public MemberType Type => MemberType.Operation;

        public int Priority { get; }
        public Associativity Order { get; set; }

        private MethodInfo method;

        public LibMethodWrapper(MethodInfo method)
        {
            this.method = method;
            ParamsCount = method.GetParameters().Length;
            Priority = 3;
        }

        public IFunctor SetInnerArgs(IEnumerable<IValueHolder> args)
        {
            if (args.Count() != ParamsCount)
            {
                throw new InvalidOperationException($"Functor {Name} have runtime " +
                    $"prototype with {Inner.Length} parameters. You passed {args.Count()}.");
            }

            Inner = args.ToArray();
            return this;
        }

        public IValueHolder Calculate()
        {
            double[] numbers = Inner.Select(i => (double)i.Value).ToArray();
            object[] values = numbers.Cast<object>().ToArray();

            var result = method.Invoke(null, values);

            return new ValueHolder(new Number((double)result));
        }
    }
}
