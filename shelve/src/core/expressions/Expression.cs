namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    public sealed class Expression
    {
        public readonly string TargetVariable;

        public readonly string InitialExpressionString;

        public readonly string VariableSet;

        internal short priority;

        internal readonly Queue<IFunctor> members;

        private Stack<IValueHolder> machine;

        public Expression(string targetVariable, string initiaExpressionString, string variableSet)
        {
            TargetVariable = targetVariable;
            InitialExpressionString = initiaExpressionString;

            machine = new Stack<IValueHolder>();
            members = new Queue<IFunctor>();
        }

        public Number Calculate()
        {
            int steps = members.Count;
            var expr = new Queue<IFunctor>(members);

            while (steps --> 0)
            {
                var member = expr.Dequeue();

                if (member.Type == MemberType.Operand)
                {
                    machine.Push(member.Calculate());
                }
                else
                {
                    var args = new IValueHolder[member.ParamsCount];

                    for (int i = args.Length - 1; i >= 0; i--)
                    {
                        args[i] = machine.Pop();
                    }

                    try
                    {
                        machine.Push(member.SetInnerArgs(args).Calculate());
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"An exception occured during calculate an expression " +
                            $"\"{InitialExpressionString}\" in variable set \"{VariableSet}\".\n" +
                            $"Original exception is {ex}");
                    }
                }
            }

            return machine.Pop().Value;
        }
    }
}
