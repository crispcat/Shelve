namespace Shelve.Tests
{
    using Shelve.Core;
    using NUnit.Framework;

    [TestFixture]
    public class ShuntingYardTests
    {
        [Test] public void InfixToPostfixNotation()
        {
            var expressionString = "expmpl += sin(13 - 3) + 0.8 ^ 11 * (pow(-1, -1.18 + 33) - 1 / (-x + 1))";

            var functionRegex = MathWrapper.GetMethodsRegex(System.Text.RegularExpressions.RegexOptions.Compiled);

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());
            var expression = translator.CreateCalculationStack();


            
        }
    }
}
