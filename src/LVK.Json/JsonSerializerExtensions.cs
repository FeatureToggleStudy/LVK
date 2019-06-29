using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    [PublicAPI]
    public static class JsonSerializerExtensions
    {
        [CanBeNull]
        public static T DeserializeObject<T>([NotNull] this JsonSerializer serializer, [NotNull] string json)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            if (json == null)
                throw new ArgumentNullException(nameof(json));
            
            using (var textReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(textReader))
                return serializer.Deserialize<T>(jsonReader);
        }

        [CanBeNull]
        public static T DeserializeObjectFromFile<T>([NotNull] this JsonSerializer serializer, [NotNull] string filePath)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (var textReader = new StreamReader(filePath, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
                return serializer.Deserialize<T>(jsonReader);
        }

        [NotNull]
        public static string SerializeObject([NotNull] this JsonSerializer serializer, [NotNull] object value)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            using (var textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, value);
                return textWriter.ToString();
            }
        }
    }
}