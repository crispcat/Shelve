using System;
using System.Collections.Generic;
using System.Text;

namespace Shelve.Core
{
    internal readonly struct Polynumber : IComparable, IComparable<Polynumber>, IEquatable<Polynumber>
    {
        private readonly HashSet<short> members;

        internal static Polynumber FromFloat(float value)
        {
            throw new NotImplementedException();
        }

        internal static Polynumber FromDouble(double value)
        {
            throw new NotImplementedException();
        }

        internal static Polynumber FromInt(long value)
        {
            throw new NotImplementedException();
        }

        internal static Polynumber Parse(string valueString)
        {
            throw new NotImplementedException();
        }

        internal static bool TryParse(string valueString, out Polynumber polynumber)
        {
            throw new NotImplementedException();
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
