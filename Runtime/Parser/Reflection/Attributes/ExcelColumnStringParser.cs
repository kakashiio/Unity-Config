using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 12:26
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExcelColumnStringParser : Attribute
    {
        public Type ParserType;

        public ExcelColumnStringParser(Type parserType)
        {
            if (parserType != null)
            {
                ParserType = parserType;
                if (!typeof(IStringParser).IsAssignableFrom(parserType))
                {
                    throw new Exception($"StringParser must implement {nameof(IStringParser)}, but {parserType} does not.");
                }   
            }
        }
    }
}