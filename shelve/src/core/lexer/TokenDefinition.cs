namespace Shelve.Core
{
    using System.Text.RegularExpressions;

    internal enum Token
    {
        Variable, Value, Binar, Unar, Function,
        LeftBracket, RightBracket, SqLeftBracket, SqRightBracket, Divider
    }

    internal sealed class TokenDefinition
    {
        public readonly ISwichableMatcher Matcher;
        public readonly Token Token;

        public TokenDefinition(Token token, Regex regex)
        {
            Matcher = new RegexMatcher(regex);
            Token = token;
        }
    }
}
