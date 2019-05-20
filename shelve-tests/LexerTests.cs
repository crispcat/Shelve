namespace Shelve.Tests
{
    using Shelve.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class LexerTests
    {
        [Test] public void CorrectExpression()
        {
            var expression1 = "exmpl+=0.1+0.8*(-1-1/(bstr_grow_restart_boost+1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expression1);

            var lexicalQueue = new Queue<Lexema>();

            lexicalQueue.Enqueue(new Lexema("exmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+=", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("bstr_grow_restart_boost", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            Assert.IsTrue(SequenceTester.IsEqual(tokens.LexicalQueue, lexicalQueue));
        }

        [Test] public void CorrectExpressionWithFunctions()
        {
            var expression1 = "expmpl += sin(0.1) + 0.8 * (pow(-1, 2) - 1 / (x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expression1);

            var lexicalQueue = new Queue<Lexema>();

            lexicalQueue.Enqueue(new Lexema("expmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+=", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("sin", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("0.1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));

            lexicalQueue.Enqueue(new Lexema("pow", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(",", Token.Divider));
            lexicalQueue.Enqueue(new Lexema("2", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("x", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            Assert.IsTrue(SequenceTester.IsEqual(tokens.LexicalQueue, lexicalQueue));
        }

        [Test] public void FunctionCorrectAsVariableName()
        {
            var expression1 = "exp += sin + 0.8 * (-1 - cos / (x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expression1);

            var lexicalQueue = new Queue<Lexema>();

            lexicalQueue.Enqueue(new Lexema("exp", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+=", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("sin", Token.Variable));

            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("cos", Token.Variable));

            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("x", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            Assert.IsTrue(SequenceTester.IsEqual(tokens.LexicalQueue, lexicalQueue));
        }

        [Test] public void ComplexCorrectExpression()
        {
            var expression = "expmpl += sin(13 - 3) + 0.8 % 11 * (pow(-1, -1.18) - 1 / (-x + 1))";

            var lexicalQueue = new Queue<Lexema>();

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expression);

            lexicalQueue.Enqueue(new Lexema("expmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+=", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("sin", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("13", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("3", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("%", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("11", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("pow", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(",", Token.Divider));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1.18", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("x", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            Assert.IsTrue(SequenceTester.IsEqual(tokens.LexicalQueue, lexicalQueue));
        }

        [Test] public void UncorrectExpression1()
        {
            var expression1 = "5 += sin(0.1) + 0.8 * (-1 - 1 / (x + 1))";

            try
            {
                var lexer = new Lexer();
                var tokens = lexer.Tokenize(expression1);

                Assert.Fail();
            }
            catch (System.ArgumentException) { }
        }

        [Test] public void UncorrectExpression2()
        {
            var expression1 = "exp += sin30.1) + 0.8 * (-1 - 1 / (x + 1))";

            try
            {
                var lexer = new Lexer();
                var tokens = lexer.Tokenize(expression1);

                Assert.Fail();
            }
            catch (System.ArgumentException) { }
        }

        [Test] public void UncorrectExpression3()
        {
            var expression1 = "exp += sin(30.1) ^ * 0.8 * (-1 - 1 / (x + 1))";

            try
            {
                var lexer = new Lexer();
                var tokens = lexer.Tokenize(expression1);

                Assert.Fail();
            }
            catch (System.ArgumentException) { }
        }

        [Test] public void UncorrectExpression4()
        {
            var expression1 = "exp += sin(30.1) ^ .8 * (-1 - 1 / (x + 1))";

            try
            {
                var lexer = new Lexer();
                var tokens = lexer.Tokenize(expression1);

                Assert.Fail();
            }
            catch (System.ArgumentException) { }
        }

        [Test] public void CorrectIterator()
        {
            var iterator = "expmpl = [1, sin(13 - 3) + 0.8 % 11 * (pow(-1, -1.18) - 1 / (-x + 1))]";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(iterator);

            var lexicalQueue = new Queue<Lexema>();

            lexicalQueue.Enqueue(new Lexema("expmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("=", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("[", Token.SqLeftBracket));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(",", Token.Divider));
            lexicalQueue.Enqueue(new Lexema("sin", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("13", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("3", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("%", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("11", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Binar));

            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("pow", Token.Function));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(",", Token.Divider));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1.18", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("x", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema("]", Token.SqRightBracket));

            Assert.IsTrue(SequenceTester.IsEqual(tokens.LexicalQueue, lexicalQueue));
        }
    }
}
