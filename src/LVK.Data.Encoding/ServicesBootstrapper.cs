using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Data.Encoding
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<IVarintCodec, VarintCodec>();
            container.RegisterMapping<IVarintEncoder, IVarintCodec>();
            container.RegisterMapping<IVarintDecoder, IVarintCodec>();

            container.Register<IBigEndianBinaryCodec, BigEndianCodec>();
            container.RegisterMapping<IBigEndianBinaryEncoder, IBigEndianBinaryCodec>();
            container.RegisterMapping<IBigEndianBinaryDecoder, IBigEndianBinaryCodec>();

            container.Register<ILittleEndianBinaryCodec, LittleEndianCodec>();
            container.RegisterMapping<ILittleEndianBinaryEncoder, ILittleEndianBinaryCodec>();
            container.RegisterMapping<ILittleEndianBinaryDecoder, ILittleEndianBinaryCodec>();

            if (BitConverter.IsLittleEndian)
            {
                container.RegisterMapping<IPlatformBinaryCodec, ILittleEndianBinaryCodec>();
                container.RegisterMapping<IPlatformBinaryEncoder, ILittleEndianBinaryCodec>();
                container.RegisterMapping<IPlatformBinaryDecoder, ILittleEndianBinaryCodec>();
            }
            else
            {
                container.RegisterMapping<IPlatformBinaryCodec, IBigEndianBinaryCodec>();
                container.RegisterMapping<IPlatformBinaryEncoder, IBigEndianBinaryCodec>();
                container.RegisterMapping<IPlatformBinaryDecoder, IBigEndianBinaryCodec>();
            }
        }
    }
}