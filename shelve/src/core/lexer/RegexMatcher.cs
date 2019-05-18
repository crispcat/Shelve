namespace Shelve.Core
{
    using System.Text.RegularExpressions;

    internal sealed class RegexMatcher : ISwichableMatcher
    {
        private readonly Regex regex;

        public bool IsActive { get; set; }

        public RegexMatcher(Regex regex)
        {
            this.regex = regex;
            IsActive = true;
        }

        public int Match(string text)
        {
            if (IsActive)
            {
                var match = regex.Match(text);

                return match.Success ? match.Length : 0;
            }

            else return 0;
        }

        public override string ToString()
        {
            return regex.ToString();
        }
    }
}
