using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:42
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExceColumnIndex : Attribute
    {
        public int Index;

        public ExceColumnIndex(int index)
        {
            Index = index;
        }
    }
}