namespace Shelve.Core
{
    using System;

    internal static class DefaultOperator
    {
        public static IFunctor Add => new Operator("Add",
            (args) => new ValueHolder(args[0].Value + args[1].Value), 2, 0, Associativity.Left);

        public static IFunctor Sub => new Operator("Sub",
            (args) => new ValueHolder(args[0].Value - args[1].Value), 2, 0, Associativity.Left);

        public static IFunctor Neg => new Operator("Neg",
            (args) => new ValueHolder(-args[0].Value), 1, 2, Associativity.Left);

        public static IFunctor Mul => new Operator("Mul",
            (args) => new ValueHolder(args[0].Value * args[1].Value), 2, 1, Associativity.Left);

        public static IFunctor Div => new Operator("Div",
            (args) => new ValueHolder(args[0].Value / args[1].Value), 2, 1, Associativity.Left);

        public static IFunctor Rdiv => new Operator("Rdiv",
            (args) => new ValueHolder(args[0].Value % args[1].Value), 2, 1, Associativity.Left);

        public static IFunctor Pow => MathWrapper.GetFunctorFor("Pow");

        public static IFunctor Eql => new VoidFunctor();

        public static IFunctor GetBySign(string sign, bool isUnar = false)
        {
            IFunctor functor;

            switch (sign)
            {
                case "+" :
                    functor = Add;
                    break;

                case "-" :
                    functor = isUnar ? Neg : Sub;
                    break;

                case "*" :
                    functor = Mul;
                    break;

                case "/" :
                    functor = Div;
                    break;

                case "%" :
                    functor = Rdiv;
                    break;

                case "^" :
                    functor = Pow;
                    break;

                default :
                    throw new ArgumentException($"Sign {sign} is not an operator.");
            }

            return functor;
        }

        public static int GetPriority(string sign, bool isUnar = false) => 
            GetBySign(sign, isUnar).Priority;

        public static Associativity GetAssociativity(string sign, bool isUnar = false) =>
            GetBySign(sign, isUnar).Order;
    }
}
