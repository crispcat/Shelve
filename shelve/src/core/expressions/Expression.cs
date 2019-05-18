namespace Shelve.Core
{
    using System.Collections.Generic;

    public sealed class Expression
    {
        internal readonly short priority;

        public readonly string TargetVariable;

        internal Queue<IFunctor> members;

        private Stack<IValueHolder> machine;

        public Expression(string name)
        {
            TargetVariable = name;
            machine = new Stack<IValueHolder>();
        }

        public Number Calculate()
        {
            int steps = members.Count;

            while (steps --> 0)
            {
                var member = members.Dequeue();

                if (member.Type == MemberType.Operand)
                {
                    machine.Push(member.Calculate());
                }
                else
                {
                    var args = new IValueHolder[member.ParamsCount];

                    for (int i = args.Length - 1; i > 0; i--)
                    {
                        args[i] = machine.Pop();
                    }

                    machine.Push(member.SetInnerArgs(args).Calculate());
                }
            }

            return machine.Pop().Value;
        }
    }
}
