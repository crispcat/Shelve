using System;
using System.Numerics;
namespace Shelve.Core
{
    public struct Lexema
    {
        public int priority;
        public string symbol;
        public Lexer.Token token;

        internal Func<BigFloat, BigFloat, BigFloat> action;

        internal Lexema(string symbol, Lexer.Token token, Func<BigFloat, BigFloat, BigFloat> action, int priority)
        {
            this.token = token;
            this.symbol = symbol;
            this.action = action;
            this.priority = priority;
        }
    }
}
