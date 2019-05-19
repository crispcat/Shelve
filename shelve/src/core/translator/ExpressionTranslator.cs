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

        public ExpressionTranslator(LexedExpression lexedExpression, HashedVariables variables)
        {
            this.variables = variables;
            this.lexedExpression = lexedExpression;

            inner = lexedExpression.LexicalQueue;
            type = Validator.Elaborate(lexedExpression);
            result = new Expression(targetVariable: inner.First.Value.Represents, lexedExpression.Initial);
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

        private void BuildExpression()
        {
            lexedExpression = new ShuntingYard(lexedExpression).ProcessInput();

            while (lexedExpression.LexicalQueue.Count != 0)
            {

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
