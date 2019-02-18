using System;
using System.Text;
using System.Numerics;
using System.Collections.Generic;

namespace Shelve.Core
{
    public static class Lexer
    {
        public enum Token
        {
            //binars
            Add,
            Sub,
            Mul,
            Div,
            Rdv,
            Pow,

            //unars
            Inv,
            Sqrt,
        }

        public static Dictionary<char, Lexema> binars = new Dictionary<char, Lexema>()
        {
            {
                '+',
                new Lexema("+", Token.Add, (v1, v2) => v1 + v2, priority: 4)
            },
            {
                '-',
                new Lexema("-", Token.Sub, (v1, v2) => v1 - v2, priority: 4)
            },
            {
                '*',
                new Lexema("*", Token.Mul, (v1, v2) => v1 * v2, priority: 3)
            },
            {
                '/',
                new Lexema("/", Token.Div, (v1, v2) => v1 / v2, priority: 2)
            },
            {
                '%',
                new Lexema("%", Token.Rdv, (v1, v2) => BigFloat.Round(v1 / v2), priority: 2)
            },
            {
                '^',
                new Lexema("^", Token.Pow, (v1, v2) => BigFloat.Pow(v1, (int)(float)v2), priority: 3)
            }
        };

        public static Dictionary<string, Lexema> unars = new Dictionary<string, Lexema>()
        {
            {
                "-",
                new Lexema("-", Token.Inv, (v1, v2) => -v1, priority: 5)
            },
            {
                "sqrt",
                new Lexema("sqrt", Token.Sqrt, (v1, v2) => BigFloat.Sqrt(v1), priority: 5)
            }
        };

        public static HashSet<char> pass = new HashSet<char>()
        {
            '(', ')'
        };


    }
}
