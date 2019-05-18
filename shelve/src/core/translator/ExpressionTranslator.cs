namespace Shelve.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class ExpressionTranslator
    {
        private Type type;
        private Expression result;
        private Queue<Lexema> inner;
        private HashedVariables variables;
        private LexedExpression lexedExpression;

        private Lexema variableToken;
        private Lexema assignOperatorToken;

        public ExpressionTranslator(LexedExpression lexedExpression, HashedVariables variables)
        {
            this.variables = variables;
            this.lexedExpression = lexedExpression;

            type = Validator.Elaborate(lexedExpression);
            inner = new Queue<Lexema>(lexedExpression.LexicalQueue);
            result = new Expression(inner.Peek().Represents);
        }

        public Expression CreateCalculationStack()
        {
            variableToken = inner.Dequeue();
            assignOperatorToken = inner.Dequeue();

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
            inner = DisposeIteratorDeclaration(inner);

            var initialValue = inner.Dequeue();
            IValueHolder iterator;

            if (initialValue.Token == Token.Value)
            {
                iterator = variables.CreateIterator(variableToken.Represents, Number.Parse(initialValue.Represents));
            }
            else
            {
                iterator = variables.CreateIterator(variableToken.Represents);
            }
        }

        private Queue<Lexema> DisposeIteratorDeclaration(Queue<Lexema> lexemas)
        {
            var tokenArr = lexemas.ToList();

            tokenArr.RemoveAt(0);
            tokenArr.RemoveAt(1);
            tokenArr.RemoveAt(tokenArr.Count - 1);

            return new Queue<Lexema>(tokenArr);
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
                    throw new ArgumentException($"Can`t create an assignment in expression: \"{lexedExpression.Initial}\"\n" +
                        $"Set: \"{lexedExpression.TargetSet}\".\n Reason: {ex.ToString()}");
                }

                var flow = inner.ToArray();

                inner.Clear();
                inner.Enqueue(variableToken);
                inner.Enqueue(assignOperatorToken);

                foreach (var lexema in flow)
                {
                    inner.Enqueue(lexema);
                }
            }
        }

        private Queue<Lexema> ShuntInnerFlow()
        {
            var shuntingYard = new ShuntingYard(inner);

            return shuntingYard.ProcessInput();
        }

        private void BuildExpression()
        {
            
        }
    }
}
