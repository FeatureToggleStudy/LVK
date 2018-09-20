using System;

using JetBrains.Annotations;

using LVK.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.Json
{
    [PublicAPI]
    public static class JsonBuilder
    {
        [NotNull]
        public static JToken ValueFromString([NotNull] string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                return JToken.Parse(value) ?? new JValue(string.Empty);
            }
            catch (JsonReaderException)
            {
                return new JValue(value);
            }
        }

        [NotNull]
        public static JObject Construct([NotNull, ItemNotNull] string[] path, [NotNull] JToken value)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var root = new JObject();
            var current = root;
            for (int index = 0; index < path.Length - 1; index++)
            {
                var child = new JObject();
                current[path[index]] = child;
                current = child;
            }

            current[path[path.Length - 1]] = value;
            return root;
        }

        public static void Apply([NotNull] JObject source, [NotNull] JObject target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var child = source.First;
            while (child != null)
            {
                if (child is JProperty prop)
                    OverrideProperty(target, prop);
                else
                    throw new InvalidOperationException($"unknown: '{child.GetType().Name}'");

                child = child.Next;
            }
        }

        private static void OverrideProperty([NotNull] JObject target, [NotNull] JProperty prop)
        {
            JetBrainsHelpers.assume(prop.Name != null);

            if (!target.ContainsKey(prop.Name))
            {
                target[prop.Name] = prop.Value;
                return;
            }

            JetBrainsHelpers.assume(target[prop.Name] != null);
            if (prop.Value?.GetType() != target[prop.Name].GetType())
                target[prop.Name] = prop.Value;
            else
                OverrideValue(target, prop);
        }

        private static void OverrideValue([NotNull] JObject target, [NotNull] JProperty prop)
        {
            JetBrainsHelpers.assume(prop.Name != null);

            switch (prop.Value)
            {
                case null:
                    break;

                case JValue val:
                    target[prop.Name] = val;
                    break;

                case JArray a:
                    target[prop.Name] = a;
                    break;

                case JObject obj:
                    Apply(obj, (JObject)target[prop.Name].NotNull());
                    break;

                default:
                    throw new InvalidOperationException($"unknown value-type: '{prop.Value.GetType().Name}'");
            }
        }
    }
}