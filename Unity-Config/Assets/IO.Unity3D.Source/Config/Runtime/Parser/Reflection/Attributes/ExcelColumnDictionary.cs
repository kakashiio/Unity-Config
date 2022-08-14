using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 13:04
    //******************************************
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class ExcelColumnDictionary : Attribute
    {
        public const string DEFAULT_ITEM_SEPARATOR = ";";
        public const string DEFAULT_KV_SEPARATOR = ",";
        
        public string ItemSeparator = DEFAULT_ITEM_SEPARATOR;
        public string KVSeparator = DEFAULT_KV_SEPARATOR;
        public Type KeyParserType;
        public Type ValueParserType;
        
        public ExcelColumnDictionary(string itemSeparator = DEFAULT_ITEM_SEPARATOR, string kvSeparator = DEFAULT_KV_SEPARATOR)
        {
            ItemSeparator = itemSeparator;
            KVSeparator = kvSeparator;
        }
    }
}