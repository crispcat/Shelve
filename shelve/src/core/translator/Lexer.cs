namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal static class Lexer
    {
        /// <summary>
        /// Translate infix string expression in postfix lexical stack
        /// </summary>
        public static Stack<Lexema> TanslateToLexicalStack(string expression)
        {
            var symbolicFlow = new Queue<char>(expression);

            throw new NotImplementedException();

            //while (symbolicFlow.Count != 0)
            //{

            //}
        }
    }
}
