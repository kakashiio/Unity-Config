using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:43
    //******************************************    
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExcelColumnName : Attribute
    {
        public string Name;

        public ExcelColumnName(string name)
        {
            Name = name;
        }
    }
}