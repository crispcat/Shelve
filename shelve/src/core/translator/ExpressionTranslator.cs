namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal class ExpressionTranslator
    {
        private readonly Type type;
        private readonly Expression result;

        private LinkedList<Lexema> inner;
        private HashedVariables variables;
        private LexedExpression lexedExpression;

        private Lexema variableToken;
        private Lexema assignOperatorToken;

        private bool canCreateInstances;

        public ExpressionTranslator(LexedExpression lexedExpression, HashedVariables variables)
        {
            this.variables = variables;
            this.lexedExpression = lexedExpression;

            inner = lexedExpression.LexicalQueue;
            type = Validator.Elaborate(lexedExpression);
            result = new Expression(inner.First.Value.Represents, lexedExpression.Initial, lexedExpression.TargetSet);

            canCreateInstances = false;
        }

        public Expression CreateCalculationStack()
        {
            variableToken = inner.DequeueHead();
            assignOperatorToken = inner.DequeueHead();

            if (type == typeof(Iterator))
            {
                PresetIterator();
            }
            else
            {
                PresetVariable();
            }

            CreateAssignment();
            ShuntExpression();
            BuildExpression();

            return result;
        }

        private void PresetIterator()
        {
            inner.DequeueHead(); //Drops "["

            var initialValue = inner.DequeueHead();
            IValueHolder iterator;

            if (initialValue.Token == Token.Value)
            {
                iterator = variables.CreateIterator(variableToken.Represents, Number.Parse(initialValue.Represents));
            }
            else
            {
                iterator = variables.CreateIterator(variableToken.Represents);
            }

            inner.DequeueHead(); //Drops divider
            inner.DequeueTail(); //Drops "]"
        }

        private void PresetVariable()
        {
            variables.CreateVariable(variableToken.Represents);
        }

        private void CreateAssignment()
        {
            var oprCharArray = assignOperatorToken.Represents.ToCharArray();

            if (oprCharArray.Length > 1)
            {
                try
                {
                    var assignOperator = DefaultOperator.GetBySign(oprCharArray[0].ToString());
                    result.priority = (short)assignOperator.Priority;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"Can`t create an assignment in expression: \"{lexedExpression.Initial}\". " +
                        $"Set: \"{lexedExpression.TargetSet}\". Reason: {ex.ToString()}");
                }

                inner.EnqueueHead(new Lexema(oprCharArray[0].ToString(), Token.Binar));
                inner.EnqueueHead(variableToken);
            }
        }

        private void ShuntExpression()
        {
            lexedExpression = new ShuntingYard(lexedExpression).ProcessInput();
        }

        private void BuildExpression()
        {
            if (LexicalQueue.Count == 1 && LexicalQueue.First.Value.Token == Token.Value)
            {
                variables[variableToken.Represents].Value = Number.Parse(LexicalQueue.First.Value.Represents);

                return;
            }

            for (int i = 0; i < lexedExpression.LexicalQueue.Count; i++)
            {
                var lexema = lexedExpression.LexicalQueue.DequeueHead();

                IFunctor functor;

                switch (lexema.Token)
                {
                    case Token.Value:

                        functor = new VoidFunctor().SetInnerArgs(new IValueHolder[]
                        {
                            new ValueHolder(Number.Parse(lexema.Represents))
                        });
                        break;

                    case Token.Variable:

                        if (variables.getters.ContainsKey(lexema.Represents))
                        {
                            functor = variables.getters[lexema.Represents];
                        }
                        else
                        {
                            functor = new ValueGetter(lexema.Represents, variables);
                            variables.getters.Add(lexema.Represents, functor as ValueGetter);
                        }
                        break;

                    case Token.Function:

                        functor = MathWrapper.GetFunctorFor(lexema.Represents);
                        break;

                    case Token.Binar:

                        functor = DefaultOperator.GetBySign(lexema.Represents);
                        break;

                    case Token.Unar:

                        functor = DefaultOperator.GetBySign(lexema.Represents, isUnar: true);
                        break;

                    default:

                        throw new ArgumentException($"Wrong input token {lexema.Represents} has arived " +
                            $"while translating \"{lexedExpression.Initial}\" on position {lexema.Position}! " +
                            $"TargetSet: {lexedExpression.TargetSet}");
                }

                result.members.Enqueue(functor);

                lexedExpression.LexicalQueue.EnqueueTail(lexema);
            }
        }

        #region ForShuntingTests
        internal LinkedList<Lexema> LexicalQueue => new LinkedList<Lexema>(lexedExpression.LexicalQueue);
        #endregion
    }

    internal static class ExtendLexemasLinkendListAsQueue
    {
        public static Lexema DequeueHead(this LinkedList<Lexema> list)
        {
            var first = list.First;
            list.RemoveFirst();

            return first.Value;
        }

        public static Lexema DequeueTail(this LinkedList<Lexema> list)
        {
            var last = list.Last;
            list.RemoveLast();

            return last.Value;
        }

        public static void EnqueueHead(this LinkedList<Lexema> list, Lexema lexema) => 
            list.AddFirst(lexema);

        public static void EnqueueTail(this LinkedList<Lexema> list, Lexema lexema) => 
            list.AddLast(lexema);
    }
}
