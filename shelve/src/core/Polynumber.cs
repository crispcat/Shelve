using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Shelve.Core
{
    public readonly struct Polynumber : IComparable, IComparable<Polynumber>, IEquatable<Polynumber>
    {
        private readonly HashSet<short> mantissa;
        private readonly short reverseMagnitudeOrder;

        public static Polynumber FromFloatingPoint(double value)
        {
            var fraction = value % 1;

            throw new NotImplementedException();
        }

        public static Polynumber FromInteger(long value)
        {
            return new Polynumber(GetMantissaForValue(value), 0);
        }

        private static HashSet<short> GetMantissaForValue(long value)
        {
            var bits = new BitArray(BitConverter.GetBytes(value));
            var mantissa = new HashSet<short>();

            short order = 0;

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    mantissa.Add(order);
                }
            }

            return mantissa;
        }

        internal static Polynumber Parse(string valueString)
        {
            throw new NotImplementedException();
        }

        internal static bool TryParse(string valueString, out Polynumber polynumber)
        {
            throw new NotImplementedException();
        }

        private Polynumber(HashSet<short> mantissa, short reverseMagnitudeOrder)
        {
            this.mantissa = mantissa;
            this.reverseMagnitudeOrder = reverseMagnitudeOrder;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Polynumber other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Polynumber other)
        {
            throw new NotImplementedException();
        }
    }
}
