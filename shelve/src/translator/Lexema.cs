namespace Shelve.Core
{
    using System;

    public struct Lexema
    {
        public int priority;
        public object represents;
        public Token token;

        internal Func<Number[], Number> action;

        internal Lexema(object represents, Token token, Func<Number[], Number> action = null, int priority = 0)
        {
            this.token = token;
            this.represents = represents;
            this.action = action;
            this.priority = priority;
        }
    }
}
