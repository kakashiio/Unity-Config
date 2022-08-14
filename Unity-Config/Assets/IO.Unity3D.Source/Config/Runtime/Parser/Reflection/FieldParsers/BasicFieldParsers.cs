using IO.Unity3D.Source.Reflection;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 11:46
    //******************************************
    public class IntFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetInteger(index);
        }
    }
    
    public class BoolFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetBool(index);
        }
    }
    
    public class ByteFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetByte(index);
        }
    }
    
    public class DoubleFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetDouble(index);
        }
    }
    
    public class FloatFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetFloat(index);
        }
    }
    
    public class LongFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetLong(index);
        }
    }
    
    public class ShortFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetShort(index);
        }
    }
    
    public class StringFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetString(index);
        }
    }
    
    public class SignedByteFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetSignedByte(index);
        }
    }
    
    public class UnsignedIntegerFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetUnsignedInteger(index);
        }
    }
    
    public class UnsignedLongFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetUnsignedLong(index);
        }
    }
    
    public class UnsignedShortFieldParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            return row.GetUnsignedShort(index);
        }
    }
}