using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 14:14
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelKey : Attribute
    {
        public int ID;
        public Type KeyType;
        public string KeyPropNameAlias;
    }
}