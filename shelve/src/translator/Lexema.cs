using System;
using System.Numerics;
namespace Shelve.Core
{
    public struct Lexema
    {
        public int priority;
        public object represents;
        public Token token;

        internal Func<BigFloat[], BigFloat> action;

        internal Lexema(object represents, Token token, Func<BigFloat[], BigFloat> action = null, int priority = 0)
        {
            this.token = token;
            this.represents = represents;
            this.action = action;
            this.priority = priority;
        }
    }
}
