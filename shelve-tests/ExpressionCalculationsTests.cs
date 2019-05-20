namespace Shelve.Tests
{
    using System;
    using Shelve.Core;
    using NUnit.Framework;

    [TestFixture]
    public class ExpressionCalculationsTests
    {
        [Test] public void SimpleStaticExpression()
        {
            var expressionString = "x = 5 + 3 * 2 ^ 2";
            var variables = new HashedVariables();

            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            Assert.IsTrue(variables["x"].Calculate() == 5 + 3 * Math.Pow(2, 2));
        }

        [Test] public void StaticExpressionWithFunctions()
        {
            var expressionString = "x = 5 + sin(3) * pow(2,3)";
            var variables = new HashedVariables();

            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            Assert.IsTrue(variables["x"].Calculate() == 5 + Math.Sin(3) * Math.Pow(2, 3));
        }

        [Test] public void StaticComplexExpression()
        {
            var expressionString = "x = (5 + sin(tan(3)) * pow(2, abs(3))) % 3 + (log(17, 2) - 13)";

            var variables = new HashedVariables();

            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            var libResult = variables["x"].Calculate();
            var langResult = ((int)(5 + Math.Sin(Math.Tan(3)) * Math.Pow(2, Math.Abs(3))) / 3) + (Math.Log(17, 2) - 13);

            Assert.IsTrue(libResult == langResult);
        }

        [Test] public void DynamicExpressionsAndIteration()
        {
            var exprString1 = "x += [0, 1]";
            var exprString2 = "y = 2 ^ x";

            var variables = new HashedVariables();

            var tokens1 = new Lexer().Tokenize(exprString1);
            var expression1 = new ExpressionTranslator(tokens1, variables).CreateCalculationStack();

            var tokens2 = new Lexer().Tokenize(exprString2);
            var expression2 = new ExpressionTranslator(tokens2, variables).CreateCalculationStack();

            variables[expression1.TargetVariable].AddExpression(expression1);
            variables[expression2.TargetVariable].AddExpression(expression2);

            Assert.IsTrue(variables["y"].Calculate() == 1);

            var iterator = variables["x"] as Iterator;
            iterator.MoveNextValue();

            Assert.IsTrue(variables["y"].Calculate() == 2);
            iterator.MoveNextValue();
            Assert.IsTrue(variables["y"].Calculate() == 4);
            iterator.MoveNextValue();
            Assert.IsTrue(variables["y"].Calculate() == 8);
            iterator.MoveNextValue();
            Assert.IsTrue(variables["y"].Calculate() == 16);
        }

        [Test] public void DynamicDependendentVariables()
        {
            var exprString1 = "x = 0";
            var exprString2 = "y = 2 ^ x";

            var variables = new HashedVariables();

            var tokens1 = new Lexer().Tokenize(exprString1);
            var expression1 = new ExpressionTranslator(tokens1, variables).CreateCalculationStack();

            var tokens2 = new Lexer().Tokenize(exprString2);
            var expression2 = new ExpressionTranslator(tokens2, variables).CreateCalculationStack();

            variables[expression1.TargetVariable].AddExpression(expression1);
            variables[expression2.TargetVariable].AddExpression(expression2);

            Assert.IsTrue(variables["y"].Calculate() == 1);

            var x = variables["x"];

            x.Value = 1;
            Assert.IsTrue(variables["y"].Calculate() == 2);

            x.Value = 2;
            Assert.IsTrue(variables["y"].Calculate() == 4);

            x.Value = 3;
            Assert.IsTrue(variables["y"].Calculate() == 8);

            x.Value = 4;
            Assert.IsTrue(variables["y"].Calculate() == 16);
        }

        [Test] public void SingleAggregateAssignment()
        {
            var expressionString = "x += 5";
            var variables = new HashedVariables();

            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            Assert.IsTrue(variables["x"].Calculate() == 5);
        }

        [Test] public void MultiplyAggregateAssignment()
        {
            var expressionString1 = "x += 5";
            var expressionString2 = "x += 5";
            var expressionString3 = "x *= 5";

            var variables = new HashedVariables();

            var tokens1 = new Lexer().Tokenize(expressionString1);
            var expression1 = new ExpressionTranslator(tokens1, variables).CreateCalculationStack();
            var tokens2 = new Lexer().Tokenize(expressionString2);
            var expression2 = new ExpressionTranslator(tokens2, variables).CreateCalculationStack();
            var tokens3 = new Lexer().Tokenize(expressionString3);
            var expression3 = new ExpressionTranslator(tokens3, variables).CreateCalculationStack();

            variables[expression1.TargetVariable].AddExpression(expression1);
            variables[expression2.TargetVariable].AddExpression(expression2);
            variables[expression3.TargetVariable].AddExpression(expression3);

            Assert.IsTrue(variables["x"].Calculate() == 50);
        }

        [Test] public void DivideByZero()
        {
            var expressionString = "x = 5 / 0";

            var variables = new HashedVariables();
            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            Assert.IsTrue(variables["x"].Calculate() == double.PositiveInfinity);
        }

        [Test] public void Overflow()
        {
            var expressionString = "x ^= [2, x]";

            var variables = new HashedVariables();
            var tokens = new Lexer().Tokenize(expressionString);
            var expression = new ExpressionTranslator(tokens, variables).CreateCalculationStack();

            variables[expression.TargetVariable].AddExpression(expression);

            var iterator = variables["x"] as Iterator;
            iterator.MoveNextValue();
            iterator.MoveNextValue();
            iterator.MoveNextValue();

            Assert.IsTrue(iterator.Value == double.PositiveInfinity);
        }
    }
}
