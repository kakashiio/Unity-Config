using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 12:03
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExcelColumnArraySeparator : Attribute
    {
        public const string DEFAULT_SEPARATOR = ";";
        
        public string Separator = DEFAULT_SEPARATOR;
        public ExcelColumnArraySeparator(string separator = DEFAULT_SEPARATOR)
        {
            Separator = separator;
        }
    }
}