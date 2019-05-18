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

        public Number(int value)
        {
            this.value = value;
        }

        public Number(float value)
        {
            this.value = value;
        }

        public Number(Number number)
        {
            value = number.value;
        }

        public static implicit operator double(Number number) => number.value;

        public static Number operator + (Number left, Number right)
        {
            return new Number(left.value + right.value);
        }

        public static Number operator - (Number left, Number right)
        {
            return new Number(left.value - right.value);
        }

        public static Number operator - (Number right)
        {
            return new Number(-right.value);
        }

        public static Number operator * (Number left, Number right)
        {
            return new Number(left.value * right.value);
        }

        public static Number operator / (Number left, Number right)
        {
            return new Number(left.value / right.value);
        }

        public static Number operator % (Number left, Number right)
        {
            return new Number((double)(int)(left.value / right.value));
        }

        public static implicit operator Number(int v) => (double)v;

        public static implicit operator Number(float v) => (double)v;

        public static implicit operator Number(double v) => new Number(v);

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
            if (double.TryParse(key, out double dValue))
            {
                value = new Number(dValue);
                return true;
            }
            else
            {
                value = new Number(double.NaN);
                return false;
            }
        }

        internal static Number Parse(string key) => double.Parse(key);
    }
}
