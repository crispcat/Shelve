namespace Shelve.Core
{
    using System;

    [Serializable]
    public readonly struct Number : IComparable<Number>, IEquatable<Number>
    {
        private readonly double value;

        public Number(double value)
        {
            this.value = value;
        }

        public Number(Number number)
        {
            value = number.value;
        }

        public static Number operator + (Number left, Number right)
        {
            return new Number(left.value + right.value);
        }

        public static Number operator - (Number left, Number right)
        {
            return new Number(left.value - right.value);
        }

        public static Number operator * (Number left, Number right)
        {
            return new Number(left.value * right.value);
        }

        public static Number operator / (Number left, Number right)
        {
            return new Number(left.value / right.value);
        }

        public int CompareTo(Number other)
        {
            return value == other.value ? 0 : value > other.value ? 1 : -1;
        }

        public bool Equals(Number other)
        {
            return value == other.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        internal static bool TryParse(string key, out Number value)
        {
            throw new NotImplementedException();
        }
    }
}
