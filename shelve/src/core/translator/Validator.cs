﻿namespace Shelve.Core
{
    using System;
    using System.Linq;

    internal static class Validator
    {
        public static Type Elaborate(LexedExpression expression)
        {
            var tokens = expression.LexicalQueue.ToArray();
            var operatorString = tokens.First(t => t.Token == Token.Binar).Represents;

            if (!operatorString.Contains("="))
            {
                throw new Exception($"Assign operator expected in expression \"{expression.Initial}\"." +
                    $"VariableSet: \"{expression.TargetSet}\"");
            }

            var sqBrackets = tokens.Where(t => t.Token == Token.SqLeftBracket || t.Token == Token.SqRightBracket);

            bool isIterator = sqBrackets.Count() != 0;

            bool isCorrectIterator = sqBrackets.First().Token == Token.SqLeftBracket && sqBrackets.Last().Token == Token.SqRightBracket;

            if (isIterator)
            {
                if (isCorrectIterator)
                {
                    return typeof(Iterator);
                }
                else
                {
                    throw new Exception($"Iterator definition in expression " +
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
                throw new Exception($"Target variable declaration expected " +
                    $"as first element of expression \"{expression.Initial}\"." +
                    $"VariableSet: \"{expression.TargetSet}\"");
            }
        }
    }
}
