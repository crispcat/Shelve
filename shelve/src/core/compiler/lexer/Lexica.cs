namespace Shelve.Core
{
    using System;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class Lexica
    {
        #region Regex
        private static readonly RegexOptions options = RegexOptions.Compiled;

        private static readonly Regex variableRegex = new Regex(@"^[a-zA-Z_]+[a-zA-Z_0-9]*", options);
        private static readonly Regex valueRegex = new Regex(@"^[+-]?\d+([\.\,]\d+)?([eE][+-]?\d+)?", options);
        private static readonly Regex operatorRegex = new Regex(@"^(\+|\-|\*|\/|\%|\^|\=)(\=)?", options);
        private static readonly Regex leftBracketRegex = new Regex(@"^\(", options);
        private static readonly Regex rightBracketRegex = new Regex(@"^\)", options);
        private static readonly Regex sqLeftBracketRegex = new Regex(@"^\[", options);
        private static readonly Regex sqRightBracketRegex = new Regex(@"^\]", options);

        private static Regex GetFunctionsRegex()
        {
            var bindingFlags = BindingFlags.Static | BindingFlags.Public;
            var reflectedMethods = typeof(Math).GetMethods(bindingFlags);
            var doubleMethods = new List<MethodInfo>();

            foreach (var method in reflectedMethods)
            {
                if (method.ReturnType == typeof(double))
                {
                    doubleMethods.Add(method);
                }
            }

            var regularExpression = new StringBuilder();

            foreach (var method in doubleMethods)
            {
                regularExpression.Append($"{method.Name.ToLower()}|");
            }

            regularExpression.Remove(regularExpression.Length - 1, 1);
            regularExpression.Append(@"(\()");

            return new Regex(regularExpression.ToString(), options);
        }
        #endregion

        public TokenDefinition[] Rules { get; }

        public Lexica()
        {
            Rules = new TokenDefinition[]
            {
                new TokenDefinition(Token.Variable, variableRegex),                 //0
                new TokenDefinition(Token.Value, valueRegex),                       //1
                new TokenDefinition(Token.Operator, operatorRegex),                 //2
                new TokenDefinition(Token.LeftBracket, leftBracketRegex),           //3
                new TokenDefinition(Token.RightBracket, rightBracketRegex),         //4
                new TokenDefinition(Token.SqLeftBracket, sqLeftBracketRegex),       //5
                new TokenDefinition(Token.SqRightBracket, sqRightBracketRegex),     //6
                //new TokenDefinition(Token.Function, GetFunctionsRegex())            //7
            };
        }

        public void SwitchOffRules(IEnumerable<int> indexes)
        {
            foreach (var index in indexes)
            {
                Rules[index].Matcher.IsActive = false;
            }
        }

        public void SwitchOnRules(IEnumerable<int> indexes)
        {
            foreach (var index in indexes)
            {
                Rules[index].Matcher.IsActive = true;
            }
        }
    }
}
