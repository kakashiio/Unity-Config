using System.Collections;
using System.Collections.Generic;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-14 13:53
    //******************************************
    public class StaticExcelStream : IExcelStream
    {
        private IExcel _Excel;
        
        public StaticExcelStream(IExcel excel)
        {
            _Excel = excel;
        }

        public IEnumerator<IExcelRow> GetEnumerator()
        {
            return new Enumerator(_Excel);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        class Enumerator: IEnumerator<IExcelRow>
        {
            private IExcel _Excel;
            private int _Index = -1;

            public Enumerator(IExcel excel)
            {
                _Excel = excel;
            }

            public bool MoveNext()
            {
                _Index++;
                return _Index < _Excel.RowCount;
            }

            public void Reset()
            {
                _Index = -1;
            }

            public IExcelRow Current => _Excel.GetRow(_Index);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}