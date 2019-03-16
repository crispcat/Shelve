namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a value, variable name or operator from expression string
    /// </summary>
    internal struct Lexema
    {
        public enum Token
        {
            //binars
            Add, Sub, Mul, Div, Rdv, Pow,

            //unars
            Inv, Sqrt, Round,

            //entities
            Value, Variable,

            //syntax
            BL, BR, SBL, SBR
        }

        public Token token;
        public int priority;
        public object represents;
        internal Func<Number[], Number> action;

        private Lexema(object represents, Token token, Func<Number[], Number> action = null, int priority = 0)
        {
            this.token = token;
            this.represents = represents;
            this.action = action;
            this.priority = priority;
        }

        private static Dictionary<string, Lexema> syntaxis = new Dictionary<string, Lexema>()
        {
            {
                "+",
                new Lexema("+", Token.Add, (args) => args[0] + args[1], priority: 4)
            },
            {
                "-",
                new Lexema("-", Token.Sub, (args) => args[0] - args[1], priority: 4)
            },
            {
                "*",
                new Lexema("*", Token.Mul, (args) => args[0] * args[1], priority: 3)
            },
            {
                "/",
                new Lexema("/", Token.Div, (args) => args[0] / args[1], priority: 3)
            },
            {
                "(",
                new Lexema("(", Token.BL)
            },
            {
                ")",
                new Lexema(")", Token.BR)
            },
            {
                "[",
                new Lexema("[", Token.SBL)
            },
            {
                "]",
                new Lexema("]", Token.SBR)
            }
        };

        public static Lexema GetLexemaFor(string key)
        {
            Lexema result;

            if (syntaxis.ContainsKey(key))
            {
                result = syntaxis[key];
            }
            else if (double.TryParse(key, out double value))
            {
                result = new Lexema(value, Token.Value);
            }
            else
            {
                return new Lexema(key, Token.Variable);
            }

            return result;
        }
    }
}
