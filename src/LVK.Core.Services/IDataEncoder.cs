using System.IO;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IDataEncoder
    {
        void WriteInt32([NotNull] Stream target, int value);
        int ReadInt32([NotNull] Stream source);
        void WriteUInt32([NotNull] Stream target, uint value);
        uint ReadUInt32([NotNull] Stream source);
    }
}