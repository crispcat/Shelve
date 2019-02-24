namespace Shelve.Core
{
    using System;

    public struct Lexema
    {
        public int priority;
        public object represents;
        public Token token;

        internal Func<double[], double> action;

        internal Lexema(object represents, Token token, Func<double[], double> action = null, int priority = 0)
        {
            this.token = token;
            this.represents = represents;
            this.action = action;
            this.priority = priority;
        }
    }
}
