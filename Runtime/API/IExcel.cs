namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 15:32
    //******************************************
    public interface IExcel
    {
        public int RowCount { get; }
        public IExcelRow GetRow(int index);
    }
}