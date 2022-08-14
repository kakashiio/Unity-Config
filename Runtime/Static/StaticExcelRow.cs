namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:03
    //******************************************
    public class StaticExcelRow : IExcelRow
    {
        private object[] _ColumnDatas;

        public int ColumnCount => _ColumnDatas == null ? 0 : _ColumnDatas.Length;

        public StaticExcelRow(object[] columnDatas)
        {
            _ColumnDatas = columnDatas;
        }

        public bool GetBool(int index)
        {
            return (bool)_ColumnDatas[index];
        }

        public byte GetByte(int index)
        {
            return (byte)_ColumnDatas[index];
        }

        public sbyte GetSignedByte(int index)
        {
            return (sbyte)_ColumnDatas[index];
        }

        public short GetShort(int index)
        {
            return (short)_ColumnDatas[index];
        }

        public ushort GetUnsignedShort(int index)
        {
            return (ushort)_ColumnDatas[index];
        }

        public int GetInteger(int index)
        {
            return (int)_ColumnDatas[index];
        }

        public uint GetUnsignedInteger(int index)
        {
            return (uint)_ColumnDatas[index];
        }

        public long GetLong(int index)
        {
            return (long)_ColumnDatas[index];
        }

        public ulong GetUnsignedLong(int index)
        {
            return (ulong)_ColumnDatas[index];
        }

        public float GetFloat(int index)
        {
            return (float)_ColumnDatas[index];
        }

        public double GetDouble(int index)
        {
            return (double)_ColumnDatas[index];
        }

        public string GetString(int index)
        {
            return (string)_ColumnDatas[index];
        }

        public object GetRaw(int index)
        {
            return _ColumnDatas[index];
        }
    }
}