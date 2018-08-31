using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    class Configuration : IConfiguration
    {
        [NotNull]
        private readonly JToken _Root;

        public Configuration([NotNull] JToken root)
        {
            _Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public IConfiguration this[string path] => GetSection(path.Split('/'));

        public IConfiguration this[string[] path] => GetSection(path);

        [NotNull]
        private IConfiguration GetSection([NotNull, ItemNotNull] string[] path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var current = _Root;
            foreach (var element in path)
            {
                var child = current[element];
                if (child == null)
                    return new Configuration(new JObject());

                current = child;
            }

            return new Configuration(current);
        }

        public T Value<T>()
        {
            if (!(_Root is JObject obj))
                return _Root.Value<T>();

            try
            {
                return obj.ToObject<T>();
            }
            catch (ArgumentException)
            {
                return default;
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        public override string ToString() => _Root.ToString();
    }
}