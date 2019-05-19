namespace Shelve.Core
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class Lexica
    {
        #region Regex
        private static readonly RegexOptions options = RegexOptions.Compiled;

        public static readonly Regex variableRegex = new Regex(@"^[a-zA-Z_]+[a-zA-Z_0-9]*", options);
        public static readonly Regex valueRegex = new Regex(@"^\d+([\.]\d+)?([eE][+-]?\d+)?", options);
        public static readonly Regex binarRegex = new Regex(@"^(\+|\-|\*|\/|\%|\^|\=)(\=)?", options);
        public static readonly Regex unarRegex = new Regex(@"^^[\+\-]", options);
        public static readonly Regex leftBracketRegex = new Regex(@"^\(", options);
        public static readonly Regex rightBracketRegex = new Regex(@"^\)", options);
        public static readonly Regex sqLeftBracketRegex = new Regex(@"^\[", options);
        public static readonly Regex sqRightBracketRegex = new Regex(@"^\]", options);
        public static readonly Regex dividerRegex = new Regex(@"^\,");
        #endregion

        public TokenDefinition[] Rules { get; }

        public Lexica()
        {
            Rules = new TokenDefinition[]
            {
                new TokenDefinition(Token.Function, MathWrapper.GetMethodsRegex(options)), //0
                new TokenDefinition(Token.Unar, unarRegex),                                //1
                new TokenDefinition(Token.Variable, variableRegex),                        //2
                new TokenDefinition(Token.Value, valueRegex),                              //3
                new TokenDefinition(Token.Binar, binarRegex),                              //4
                new TokenDefinition(Token.LeftBracket, leftBracketRegex),                  //5
                new TokenDefinition(Token.RightBracket, rightBracketRegex),                //6
                new TokenDefinition(Token.SqLeftBracket, sqLeftBracketRegex),              //7
                new TokenDefinition(Token.SqRightBracket, sqRightBracketRegex),            //8
                new TokenDefinition(Token.Divider, dividerRegex)                           //9
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
