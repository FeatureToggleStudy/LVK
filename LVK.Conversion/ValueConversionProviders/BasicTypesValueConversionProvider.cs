using System;

namespace LVK.Conversion.ValueConversionProviders
{
    internal class BasicTypesValueConversionProvider : SimpleConversionProviderBase
    {
        public BasicTypesValueConversionProvider()
        {
            AddConverter((int i, IFormatProvider fp) => (uint)i);
            AddConverter((uint i, IFormatProvider fp) => (int)i);
            AddConverter((int i, IFormatProvider fp) => i.ToString(fp));
        }
    }
}