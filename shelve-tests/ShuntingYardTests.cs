namespace Shelve.Tests
{
    using System;
    using Shelve.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ShuntingYardTests
    {
        [Test] public void InfixToPostfixNotation1()
        {
            var expressionString = "expmpl += sin(cos(13 - 3)) + 0.8 ^ 11";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());
            translator.CreateCalculationStack();

            var lexicalQueue = new Queue<Lexema>();
            lexicalQueue.Enqueue(new Lexema("expmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("13", Token.Value));
            lexicalQueue.Enqueue(new Lexema("3", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("cos", Token.Function));
            lexicalQueue.Enqueue(new Lexema("sin", Token.Function));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("11", Token.Value));
            lexicalQueue.Enqueue(new Lexema("^", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));

            Assert.IsTrue(SequenceTester.IsEqual(translator.LexicalQueue, lexicalQueue));
        }

        [Test] public void InfixToPostfixNotation2()
        {
            var expressionString = "expmpl = (pow(-1, -1.18 ^ 2 ^ 3 + z) - 1 / (-x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());
            translator.CreateCalculationStack();

            var lexicalQueue = new Queue<Lexema>();
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1.18", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("2", Token.Value));
            lexicalQueue.Enqueue(new Lexema("3", Token.Value));
            lexicalQueue.Enqueue(new Lexema("^", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("^", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("z", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("pow", Token.Function));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("x", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("-", Token.Unar));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("/", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));

            Assert.IsTrue(SequenceTester.IsEqual(translator.LexicalQueue, lexicalQueue));
        }

        [Test] public void IteratorInfixToPostfixNotation()
        {
            var expressionString = "expmpl = [0, sin(13 - 3) + 0.8 ^ 11]";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());
            translator.CreateCalculationStack();

            var lexicalQueue = new Queue<Lexema>();
            lexicalQueue.Enqueue(new Lexema("13", Token.Value));
            lexicalQueue.Enqueue(new Lexema("3", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("sin", Token.Function));
            lexicalQueue.Enqueue(new Lexema("0.8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("11", Token.Value));
            lexicalQueue.Enqueue(new Lexema("^", Token.Binar));
            lexicalQueue.Enqueue(new Lexema("+", Token.Binar));

            Assert.IsTrue(SequenceTester.IsEqual(translator.LexicalQueue, lexicalQueue));
        }

        [Test] public void UncorrectExpression1()
        {
            var expressionString = "expmpl = (pow(-1 - 1 / (-x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());

            try
            {
                translator.CreateCalculationStack();
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.ToString());
            }
        }

        [Test] public void UncorrectExpression2()
        {
            var expressionString = "expmpl = pow(-1 - 1 / (-x + 1)))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());

            try
            {
                translator.CreateCalculationStack();
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.ToString());
            }
        }

        [Test] public void UncorrectExpression3()
        {
            var expressionString = "expmpl = pow(-1 += 1 / (-x + 1)))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            try
            {
                var translator = new ExpressionTranslator(tokens, new HashedVariables());
                translator.CreateCalculationStack();
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.ToString());
            }
        }

        [Test] public void UncorrectFunctionArguments1()
        {
            var expressionString = "expmpl -= (pow(-1) - 1 / (-x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());

            try
            {
                translator.CreateCalculationStack();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue((string)ex.Data["function"] == "pow");
                Assert.IsTrue((int)ex.Data["argsExpected"] == 2);
                Assert.IsTrue((int)ex.Data["argsRecieved"] == 1);
            }
        }

        [Test] public void UncorrectFunctionArguments2()
        {
            var expressionString = "expmpl /= (sin(pow(cos(1, 2, 3), 1)) - 1 / (-x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());

            try
            {
                translator.CreateCalculationStack();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue((string)ex.Data["function"] == "cos");
                Assert.IsTrue((int)ex.Data["argsExpected"] == 1);
                Assert.IsTrue((int)ex.Data["argsRecieved"] == 3);
            }
        }

        [Test] public void UncorrectFunctionArguments3()
        {
            var expressionString = "expmpl ^= (sin(pow(1, cos())) - 1 / (-x + 1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expressionString);

            var translator = new ExpressionTranslator(tokens, new HashedVariables());

            try
            {
                translator.CreateCalculationStack();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue((string)ex.Data["function"] == "cos");
                Assert.IsTrue((int)ex.Data["argsExpected"] == 1);
                Assert.IsTrue((int)ex.Data["argsRecieved"] == 0);
            }
        }
    }
}
