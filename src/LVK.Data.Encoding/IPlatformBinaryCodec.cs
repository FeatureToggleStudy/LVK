﻿using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface IPlatformBinaryCodec : IPlatformBinaryEncoder, IPlatformBinaryDecoder
    {
    }
}