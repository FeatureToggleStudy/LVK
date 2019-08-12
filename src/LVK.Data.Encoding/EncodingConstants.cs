namespace LVK.Data.Encoding
{
    public static class EncodingConstants
    {
        public static class Varint
        {
            public const int MaxUInt16Bytes = 3;
            public const int MaxInt16Bytes = -1;

            public const int MaxUInt32Bytes = -1;
            public const int MaxInt32Bytes = -1;

            public const int MaxUInt64Bytes = -1;
            public const int MaxInt64Bytes = -1;
        }

        public const int UInt16Bytes = 2;
        public const int Int16Bytes = 2;

        public const int UInt32Bytes = 4;
        public const int Int32Bytes = 4;

        public const int UInt64Bytes = 8;
        public const int Int64Bytes = 8;
    }
}