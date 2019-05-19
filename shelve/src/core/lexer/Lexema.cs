namespace Shelve.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a value, variable name, function or operator from expression string
    /// </summary>
    internal struct Lexema
    {
        public Token Token;
        public int Position;
        public string Represents;

        public Lexema(string represents, Token token)
        {
            Position = 0;
            Token = token;
            Represents = represents;
        }

        public static bool operator == (Lexema l1, Lexema l2)
        {
            if (l1.Token == l2.Token && l1.Represents == l2.Represents)
            {
                return true;
            }
            else return false;
        }

        public static bool operator != (Lexema l1, Lexema l2)
        {
            if (l1.Token != l2.Token || l1.Represents != l2.Represents)
            {
                return true;
            }
            else return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Lexema))
            {
                return false;
            }

            var lexema = (Lexema)obj;

            return this == lexema;
        }

        public override int GetHashCode()
        {
            var hashCode = 872051546;

            hashCode = hashCode * -1521134295 + Token.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Represents);

            return hashCode;
        }

        public override string ToString() => $"\"{Represents}\"|{Token.ToString()}|{Position}";
    }
}
