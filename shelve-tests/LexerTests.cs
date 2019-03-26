namespace Shelve.Tests
{
    using Shelve.Core;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class LexerTests
    {
        [Test] public void Tokenization()
        {
            var expression1 = "exmpl+=0,1+0,8*(-1-1/(bstr_grow_restart_boost+1))";

            var lexer = new Lexer();
            var tokens = lexer.Tokenize(expression1);

            var lexicalQueue = new Queue<Lexema>();

            lexicalQueue.Enqueue(new Lexema("exmpl", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+=", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("0,1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("+", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("0,8", Token.Value));
            lexicalQueue.Enqueue(new Lexema("*", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("-1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("-", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema("/", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("(", Token.LeftBracket));
            lexicalQueue.Enqueue(new Lexema("bstr_grow_restart_boost", Token.Variable));
            lexicalQueue.Enqueue(new Lexema("+", Token.Operator));
            lexicalQueue.Enqueue(new Lexema("1", Token.Value));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));
            lexicalQueue.Enqueue(new Lexema(")", Token.RightBracket));

            foreach (var lexema in tokens.LexicalQueue)
            {
                Assert.IsTrue(lexema == lexicalQueue.Dequeue());
            }
        }
    }
}
