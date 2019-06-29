using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Resources
{
    [PublicAPI]
    public interface IResources
    {
        [NotNull]
        Stream OpenBinary([NotNull] string name);

        [NotNull]
        TextReader OpenText([NotNull] string name, [CanBeNull] Encoding encoding = null);

        [NotNull]
        object DeserializeJson([NotNull] string name, [NotNull] Type type);
    }
}