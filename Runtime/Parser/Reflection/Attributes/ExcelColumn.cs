using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 15:13
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExcelColumn : Attribute
    {
        public enum Mode
        {
            Name,
            Index
        }

        public Mode MappingMode;
        public Type ParserType;

        public ExcelColumn(Mode mappingMode, Type parserType = null)
        {
            MappingMode = mappingMode;
            ParserType = parserType;
        }
    }
}