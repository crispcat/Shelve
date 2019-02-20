using System.Numerics;
using System.Collections.Generic;

namespace Shelve.Core
{
    public enum Token
    {
        //binars
        Add, Sub, Mul, Div, Rdv, Pow,

        //unars
        Inv, Sqrt, Round,

        //entities
        Value, Variable,
    }

    public static class Lexer
    {
        public static Dictionary<string, Lexema> funcs = new Dictionary<string, Lexema>()
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
                "%",
                new Lexema("%", Token.Rdv, (args) => BigFloat.Round(args[0] / args[1]), priority: 2)
            },
            {
                "^",
                new Lexema("^", Token.Pow, (args) => BigFloat.Pow(args[0], (int)(float)args[1]), priority: 3)
            },
            {
                "-",
                new Lexema("-", Token.Inv, (args) => -args[0], priority: 5)
            },
            {
                "sqrt",
                new Lexema("sqrt", Token.Sqrt, (args) => BigFloat.Sqrt(args[0]), priority: 5)
            },
            {
                "round",
                new Lexema("round", Token.Round, (args) => BigFloat.Round(args[0]), priority: 1)
            }
        };

        /// <summary>
        /// Translate infix string expression in postfix lexical stack
        /// </summary>
        public static Stack<Lexema> TanslateToLexicalStack(string expression)
        {
            var symbolicFlow = new Queue<char>(expression);

            while (symbolicFlow.Count != 0)
            {

            }
        }

        private static Lexema GetToken(string key)
        {
            Lexema result;

            if (funcs.ContainsKey(key))
            {
                result = funcs[key];
            }
            else if (BigFloat.TryParse(key, out BigFloat value))
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
