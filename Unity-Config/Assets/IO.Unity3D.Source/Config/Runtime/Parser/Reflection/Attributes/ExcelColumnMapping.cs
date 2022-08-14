using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 17:02
    //******************************************
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public class ExcelColumnMapping : Attribute
    {
        public string Name;
        public int Index;

    }
}