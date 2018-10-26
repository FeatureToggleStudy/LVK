using System.IO;

namespace LVK.Core.Services
{
    internal class DataEncoder : IDataEncoder
    {
        public void WriteUInt32(Stream target, uint value)
        {
            while (value != 0)
            {
                byte next = (byte)(value & 0x7f);
                value = value >> 7;

                if (value != 0)
                    next |= 0x80;
                
                target.WriteByte(next);
            }
        }
    
        public void WriteInt32(Stream target, int value) => WriteUInt32(target, unchecked((uint)value));

        public uint ReadUInt32(Stream source)
        {
            uint value = 0;
            int shift = 0;
            while (true)
            {
                int next = source.ReadByte();
                if (next < 0)
                    return value;
            
                value = value | (uint)((next & 0x7f) << shift);
                if ((next & 0x80) == 0)
                    return value;
                
                shift += 7;
            }
        }
    
        public int ReadInt32(Stream source) => unchecked((int)ReadUInt32(source));
    }
}