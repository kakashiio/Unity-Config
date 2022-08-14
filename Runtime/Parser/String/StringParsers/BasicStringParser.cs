using UnityEngine;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 12:40
    //******************************************
    public abstract class AbsStringParser : IStringParser
    {
        public object ParseString2Value(string str)
        {
            return _ParseString2Value(str == null ? null : str.Trim());
        }

        protected abstract object _ParseString2Value(string trim);
    }

    public class IntStringParser : AbsStringParser
    {
        public int ZERO = 0;

        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return int.Parse(str);
        }
    }

    public class BoolStringParser : AbsStringParser
    {
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return bool.Parse(str);
        }
    }

    public class ByteStringParser : AbsStringParser
    {
        public byte ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return byte.Parse(str);
        }
    }

    public class DoubleStringParser : AbsStringParser
    {
        public double ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return double.Parse(str);
        }
    }

    public class FloatStringParser : AbsStringParser
    {
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0f;
            }
            return float.Parse(str);
        }
    }

    public class LongStringParser : AbsStringParser
    {
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0L;
            }
            return long.Parse(str);
        }
    }

    public class ShortStringParser : AbsStringParser
    {
        public short ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return short.Parse(str);
        }
    }

    public class StringStringParser : AbsStringParser
    {
        protected override object _ParseString2Value(string str)
        {
            return str;
        }
    }

    public class SignedByteStringParser : AbsStringParser
    {
        public sbyte ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return sbyte.Parse(str);
        }
    }

    public class UnsignedIntegerStringParser : AbsStringParser
    {
        public uint ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return uint.Parse(str);
        }
    }

    public class UnsignedLongStringParser : AbsStringParser
    {
        public ulong ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return ulong.Parse(str);
        }
    }

    public class UnsignedShortStringParser : AbsStringParser
    {
        public ushort ZERO = 0;
        protected override object _ParseString2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ZERO;
            }
            return ushort.Parse(str);
        }
    }
}