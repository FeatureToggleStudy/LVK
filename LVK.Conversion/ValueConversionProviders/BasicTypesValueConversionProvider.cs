using System;

namespace LVK.Conversion.ValueConversionProviders
{
    internal class BasicTypesValueConversionProvider : SimpleConversionProviderBase
    {
        public BasicTypesValueConversionProvider()
        {
            AddSByteConverters();
            AddByteConverters();
            
            AddInt16Converters();
            AddUInt16Converters();
            
            AddInt32Converters();
            AddUInt32Converters();
            
            AddInt64Converters();
            AddUInt64Converters();

            AddCharConverters();
            AddStringConverters();

            AddBooleanConverters();
        }

        private void AddSByteConverters()
        {
            // AddConverter((sbyte i, IFormatProvider fp) => (sbyte)i);
            AddConverter((sbyte i, IFormatProvider fp) => (byte)i);
            AddConverter((sbyte i, IFormatProvider fp) => (short)i);
            AddConverter((sbyte i, IFormatProvider fp) => (ushort)i);
            AddConverter((sbyte i, IFormatProvider fp) => (int)i);
            AddConverter((sbyte i, IFormatProvider fp) => (uint)i);
            AddConverter((sbyte i, IFormatProvider fp) => (long)i);
            AddConverter((sbyte i, IFormatProvider fp) => (ulong)i);
            AddConverter((sbyte i, IFormatProvider fp) => (char)i);
            AddConverter((sbyte i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((sbyte i, IFormatProvider fp) => (double)i);
            AddConverter((sbyte i, IFormatProvider fp) => (float)i);
            AddConverter((sbyte i, IFormatProvider fp) => (decimal)i);
            AddConverter((sbyte i, IFormatProvider fp) => i != 0);
        }

        private void AddByteConverters()
        {
            AddConverter((byte i, IFormatProvider fp) => (sbyte)i);
            // AddConverter((byte i, IFormatProvider fp) => (byte)i);
            AddConverter((byte i, IFormatProvider fp) => (short)i);
            AddConverter((byte i, IFormatProvider fp) => (ushort)i);
            AddConverter((byte i, IFormatProvider fp) => (int)i);
            AddConverter((byte i, IFormatProvider fp) => (uint)i);
            AddConverter((byte i, IFormatProvider fp) => (long)i);
            AddConverter((byte i, IFormatProvider fp) => (ulong)i);
            AddConverter((byte i, IFormatProvider fp) => (char)i);
            AddConverter((byte i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((byte i, IFormatProvider fp) => (double)i);
            AddConverter((byte i, IFormatProvider fp) => (float)i);
            AddConverter((byte i, IFormatProvider fp) => (decimal)i);
            AddConverter((byte i, IFormatProvider fp) => i != 0);
        }
        
        private void AddInt16Converters()
        {
            AddConverter((short i, IFormatProvider fp) => (sbyte)i);
            AddConverter((short i, IFormatProvider fp) => (byte)i);
            // AddConverter((short i, IFormatProvider fp) => (short)i);
            AddConverter((short i, IFormatProvider fp) => (ushort)i);
            AddConverter((short i, IFormatProvider fp) => (int)i);
            AddConverter((short i, IFormatProvider fp) => (uint)i);
            AddConverter((short i, IFormatProvider fp) => (long)i);
            AddConverter((short i, IFormatProvider fp) => (ulong)i);
            AddConverter((short i, IFormatProvider fp) => (char)i);
            AddConverter((short i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((short i, IFormatProvider fp) => (double)i);
            AddConverter((short i, IFormatProvider fp) => (float)i);
            AddConverter((short i, IFormatProvider fp) => (decimal)i);
            AddConverter((short i, IFormatProvider fp) => i != 0);
        }

        private void AddUInt16Converters()
        {
            AddConverter((ushort i, IFormatProvider fp) => (sbyte)i);
            AddConverter((ushort i, IFormatProvider fp) => (byte)i);
            AddConverter((ushort i, IFormatProvider fp) => (short)i);
            // AddConverter((ushort i, IFormatProvider fp) => (ushort)i);
            AddConverter((ushort i, IFormatProvider fp) => (int)i);
            AddConverter((ushort i, IFormatProvider fp) => (uint)i);
            AddConverter((ushort i, IFormatProvider fp) => (long)i);
            AddConverter((ushort i, IFormatProvider fp) => (ulong)i);
            AddConverter((ushort i, IFormatProvider fp) => (char)i);
            AddConverter((ushort i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((ushort i, IFormatProvider fp) => (double)i);
            AddConverter((ushort i, IFormatProvider fp) => (float)i);
            AddConverter((ushort i, IFormatProvider fp) => (decimal)i);
            AddConverter((ushort i, IFormatProvider fp) => i != 0);
        }
        
        private void AddInt32Converters()
        {
            AddConverter((int i, IFormatProvider fp) => (sbyte)i);
            AddConverter((int i, IFormatProvider fp) => (byte)i);
            AddConverter((int i, IFormatProvider fp) => (short)i);
            AddConverter((int i, IFormatProvider fp) => (ushort)i);
            // AddConverter((int i, IFormatProvider fp) => (int)i);
            AddConverter((int i, IFormatProvider fp) => (uint)i);
            AddConverter((int i, IFormatProvider fp) => (long)i);
            AddConverter((int i, IFormatProvider fp) => (ulong)i);
            AddConverter((int i, IFormatProvider fp) => (char)i);
            AddConverter((int i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((int i, IFormatProvider fp) => (double)i);
            AddConverter((int i, IFormatProvider fp) => (float)i);
            AddConverter((int i, IFormatProvider fp) => (decimal)i);
            AddConverter((int i, IFormatProvider fp) => i != 0);
        }

        private void AddUInt32Converters()
        {
            AddConverter((uint i, IFormatProvider fp) => (sbyte)i);
            AddConverter((uint i, IFormatProvider fp) => (byte)i);
            AddConverter((uint i, IFormatProvider fp) => (short)i);
            AddConverter((uint i, IFormatProvider fp) => (ushort)i);
            AddConverter((uint i, IFormatProvider fp) => (int)i);
            // AddConverter((uint i, IFormatProvider fp) => (uint)i);
            AddConverter((uint i, IFormatProvider fp) => (long)i);
            AddConverter((uint i, IFormatProvider fp) => (ulong)i);
            AddConverter((uint i, IFormatProvider fp) => (char)i);
            AddConverter((uint i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((uint i, IFormatProvider fp) => (double)i);
            AddConverter((uint i, IFormatProvider fp) => (float)i);
            AddConverter((uint i, IFormatProvider fp) => (decimal)i);
            AddConverter((uint i, IFormatProvider fp) => i != 0);
        }

        private void AddInt64Converters()
        {
            AddConverter((long i, IFormatProvider fp) => (sbyte)i);
            AddConverter((long i, IFormatProvider fp) => (byte)i);
            AddConverter((long i, IFormatProvider fp) => (short)i);
            AddConverter((long i, IFormatProvider fp) => (ushort)i);
            AddConverter((long i, IFormatProvider fp) => (int)i);
            AddConverter((long i, IFormatProvider fp) => (uint)i);
            // AddConverter((long i, IFormatProvider fp) => (long)i);
            AddConverter((long i, IFormatProvider fp) => (ulong)i);
            AddConverter((long i, IFormatProvider fp) => (char)i);
            AddConverter((long i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((long i, IFormatProvider fp) => (double)i);
            AddConverter((long i, IFormatProvider fp) => (float)i);
            AddConverter((long i, IFormatProvider fp) => (decimal)i);
            AddConverter((long i, IFormatProvider fp) => i != 0);
        }

        private void AddUInt64Converters()
        {
            AddConverter((ulong i, IFormatProvider fp) => (sbyte)i);
            AddConverter((ulong i, IFormatProvider fp) => (byte)i);
            AddConverter((ulong i, IFormatProvider fp) => (short)i);
            AddConverter((ulong i, IFormatProvider fp) => (ushort)i);
            AddConverter((ulong i, IFormatProvider fp) => (int)i);
            AddConverter((ulong i, IFormatProvider fp) => (uint)i);
            AddConverter((ulong i, IFormatProvider fp) => (long)i);
            // AddConverter((ulong i, IFormatProvider fp) => (ulong)i);
            AddConverter((ulong i, IFormatProvider fp) => (char)i);
            AddConverter((ulong i, IFormatProvider fp) => i.ToString(fp));
            AddConverter((ulong i, IFormatProvider fp) => (double)i);
            AddConverter((ulong i, IFormatProvider fp) => (float)i);
            AddConverter((ulong i, IFormatProvider fp) => (decimal)i);
            AddConverter((ulong i, IFormatProvider fp) => i != 0);
        }

        private void AddCharConverters()
        {
            // AddConverter((char i, IFormatProvider fp) => (sbyte)i);
            // AddConverter((char i, IFormatProvider fp) => (byte)i);
            // AddConverter((char i, IFormatProvider fp) => (short)i);
            AddConverter((char i, IFormatProvider fp) => (ushort)i);
            AddConverter((char i, IFormatProvider fp) => (int)i);
            AddConverter((char i, IFormatProvider fp) => (uint)i);
            AddConverter((char i, IFormatProvider fp) => (long)i);
            AddConverter((char i, IFormatProvider fp) => (ulong)i);
            // AddConverter((char i, IFormatProvider fp) => (char)i);
            AddConverter((char i, IFormatProvider fp) => i.ToString(fp));
            // AddConverter((char i, IFormatProvider fp) => (double)i);
            // AddConverter((char i, IFormatProvider fp) => (float)i);
            // AddConverter((char i, IFormatProvider fp) => (decimal)i);
            AddConverter((char i, IFormatProvider fp) => i == '1');
        }

        private void AddStringConverters()
        {
            AddConverter((string s, IFormatProvider fp) => sbyte.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => byte.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => short.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => ushort.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => int.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => uint.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => long.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => ulong.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => char.Parse(s));
            // AddConverter((string s, IFormatProvider fp) => s);
            AddConverter((string s, IFormatProvider fp) => double.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => float.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => decimal.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => bool.Parse(s));
            AddConverter((string s, IFormatProvider fp) => Guid.Parse(s));
            AddConverter((string s, IFormatProvider fp) => DateTime.Parse(s, fp));
            AddConverter((string s, IFormatProvider fp) => TimeSpan.Parse(s, fp));
        }

        private void AddBooleanConverters()
        {
            AddConverter((bool b, IFormatProvider fp) => b ? (sbyte)1 : (sbyte)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (byte)1 : (byte)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (short)1 : (short)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (ushort)1 : (ushort)0);
            AddConverter((bool b, IFormatProvider fp) => b ? 1 : 0);
            AddConverter((bool b, IFormatProvider fp) => b ? (uint)1 : (uint)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (long)1 : (long)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (ulong)1 : (ulong)0);
            AddConverter((bool b, IFormatProvider fp) => b ? '1' : '0');
            AddConverter((bool b, IFormatProvider fp) => b.ToString(fp));
            AddConverter((bool b, IFormatProvider fp) => b ? (double)1 : (double)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (float)1 : (float)0);
            AddConverter((bool b, IFormatProvider fp) => b ? (decimal)1 : (decimal)0);
            // AddConverter((bool b, IFormatProvider fp) => b);
        }
    }
}