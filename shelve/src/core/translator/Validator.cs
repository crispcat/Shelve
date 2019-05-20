namespace Shelve.Core
{
    using Shelve.IO;
    using System;
    using System.Linq;

    internal static class Validator
    {
        public static Type Elaborate(LexedExpression expression)
        {
            var tokens = expression.LexicalQueue.ToArray();

            try
            {
                var operatorString = tokens.First(t => t.Token == Token.Binar).Represents;

                if (!operatorString.Contains("="))
                {
                    throw new ArgumentException($"Assign operator expected as first operator in expression \"{expression.Initial}\" " +
                        $"but operator \"{operatorString}\" pass." +
                        $"VariableSet: \"{expression.TargetSet}\"");
                }

                var assignOperators = tokens.Where(t => t.Token == Token.Binar && t.Represents.Contains("="));

                if (assignOperators.Count() > 1)
                {
                    throw new ArgumentException($"Multiple assignment in expression \"{expression.Initial}\" " +
                        $"VariableSet: \"{expression.TargetSet}\"");
                }
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException($"Operators and assignments expected in expression \"{expression.Initial}\" " +
                        $"but no one operator pass." +
                        $"VariableSet: \"{expression.TargetSet}\"");
            }

            if (expression.LexicalQueue.Count == 3)
            {
                return typeof(DynamicValueHolder);
            }

            var sqBrackets = tokens.Where(t => t.Token == Token.SqLeftBracket || t.Token == Token.SqRightBracket);

            bool isIterator = sqBrackets.Count() != 0;

            if (isIterator)
            {
                bool isCorrectIterator = sqBrackets.First().Token == Token.SqLeftBracket && 
                    sqBrackets.Last().Token == Token.SqRightBracket;

                if (isCorrectIterator)
                {
                    return typeof(Iterator);
                }
                else
                {
                    throw new ArgumentException($"Iterator definition in expression " +
                        $"\"{expression.Initial}\" expected in \" (targetVariable) (accumulateOperator) " +
                        $"[(startValue), (stepFunction)]\" format" +
                        $"VariableSet: \"{expression.TargetSet}\"");
                }
            }

            bool isVariable = Lexica.variableRegex.IsMatch(tokens.First().Represents);

            if (isVariable)
            {
                return typeof(Variable);
            }
            else
            {
                throw new ArgumentException($"Target variable declaration expected " +
                    $"as first element of expression \"{expression.Initial}\"." +
                    $"VariableSet: \"{expression.TargetSet}\"");
            }
        }

        public static void Elaborate(ParsedSet parsedSet)
        {
            if (parsedSet.Name == null)
            {
                throw new ArgumentException($"Each variable set must declare a \"Name:\" parameter");
            }
        }
    }
}
