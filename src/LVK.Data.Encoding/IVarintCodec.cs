using System;

using JetBrains.Annotations;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public interface IVarintCodec : IVarintEncoder, IVarintDecoder
    {
    }
}