using System.Collections.Generic;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:00
    //******************************************
    public class StaticExcel : IExcel
    {
        private List<IExcelRow> _Rows = new List<IExcelRow>();
        
        public int RowCount => _Rows.Count;

        public StaticExcel(List<IExcelRow> rows)
        {
            _Rows.AddRange(rows);
        }
        
        public StaticExcel(IEnumerable<IExcelRow> rows)
        {
            _Rows.AddRange(rows);
        }
        
        public StaticExcel()
        {
        }

        public StaticExcel AddRow(IExcelRow row)
        {
            if (row != null)
            {
                _Rows.Add(row);
            }
            return this;
        }

        public IExcelRow GetRow(int index)
        {
            return _Rows[index];
        }
    }
}