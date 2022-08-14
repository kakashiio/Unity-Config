using IO.Unity3D.Source.Reflection;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 11:27
    //******************************************
    public interface IExcelFieldParser
    {
        object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField);
    }
}