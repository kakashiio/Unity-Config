using System;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:16
    //******************************************
    public interface IExcelRowParser
    {
        object Parse(ExcelParserContext ctx, IExcelRow row, Type type);
    }
}