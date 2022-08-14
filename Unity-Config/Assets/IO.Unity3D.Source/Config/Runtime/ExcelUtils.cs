using System;

namespace IO.Unity3D.Source.Config
{

    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 15:09
    //******************************************
    public class ExcelUtils
    {
        private static ExcelParserContext _Ctx = new ExcelParserContext();
        
        public static void Parse<T>(IExcel excel, Action<T> onParse, ExcelParserContext ctx = null)
        {
            ctx = ctx ?? _Ctx;
            Type type = typeof(T);
            for (int i = 0; i < excel.RowCount; i++)
            {
                var row = excel.GetRow(i);
                T t = (T) ctx.Parse(row, type);
                onParse(t);
            }
        }
        public static void Parse<T>(IExcelStream excelStream, Action<T> onParse, ExcelParserContext ctx = null)
        {
            ctx = ctx ?? _Ctx;
            Type type = typeof(T);
            foreach (var row in excelStream)
            {
                T t = (T) ctx.Parse(row, type);
                onParse(t);
            }
        }
    }
}