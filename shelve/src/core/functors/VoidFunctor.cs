namespace Shelve.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class VoidFunctor : IFunctor
    {
        public string Name => Inner[0].Value.ToString();

        public int ParamsCount { get; }
        public IValueHolder[] Inner { get; private set; }

        public MemberType Type => MemberType.Operand;

        public int Priority => 4;

        public Associativity Order => Associativity.Left;

        public static IFunctor Wrap(IValueHolder valueHolder) =>
            new VoidFunctor().SetInnerArgs(new IValueHolder[] { valueHolder });

        public VoidFunctor()
        {
            ParamsCount = 1;
            Inner = new IValueHolder[1];
        }

        public IValueHolder Calculate() => Inner[0];

        public IFunctor SetInnerArgs(IEnumerable<IValueHolder> args)
        {
            if (args.Count() > 1)
            {
                throw new InvalidOperationException($"Void functor provide to receive only one argument." +
                    $"You passed {args.Count()}.");
            }

            Inner = args.ToArray();
            return this;
        }
    }
}
