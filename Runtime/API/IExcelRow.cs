namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 15:34
    //******************************************
    public interface IExcelRow
    {
        public int ColumnCount { get; }
        public bool GetBool(int index);
        
        public byte GetByte(int index);
        public sbyte GetSignedByte(int index);
        public short GetShort(int index);
        public ushort GetUnsignedShort(int index);
        public int GetInteger(int index);
        public uint GetUnsignedInteger(int index);
        public long GetLong(int index);
        public ulong GetUnsignedLong(int index);
        
        public float GetFloat(int index);
        public double GetDouble(int index);
        
        public string GetString(int index);

        public object GetRaw(int index);
    }
}