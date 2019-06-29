using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Resources
{
    [PublicAPI]
    public static class ResourcesExtensions
    {
        [NotNull]
        public static string ReadText([NotNull] this IResources resources, [NotNull] string name, [CanBeNull] Encoding encoding = null)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            using (var textReader = resources.OpenText(name, encoding))
                return textReader.ReadToEnd();
        }

        [NotNull]
        public static byte[] ReadBinary([NotNull] this IResources resources, [NotNull] string name)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            using (var stream = resources.OpenBinary(name))
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }

        [NotNull]
        public static T DeserializeJson<T>([NotNull] this IResources resources, [NotNull] string name)
            => (T)resources.DeserializeJson(name, typeof(T));
    }
}