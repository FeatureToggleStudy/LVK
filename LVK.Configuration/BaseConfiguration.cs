using System;
using System.Diagnostics;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    [DebuggerDisplay("Configuration: {" + nameof(_Path) + "}")]
    internal abstract class BaseConfiguration : IConfiguration
    {
        [NotNull]
        private readonly string _Path;

        private JObject _PreviousRootElement;
        private JToken _PreviousElement;

        protected BaseConfiguration([NotNull] string path)
        {
            _Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public IConfiguration this[string relativePath]
        {
            get
            {
                if (relativePath == null)
                    throw new ArgumentNullException(nameof(relativePath));

                return new SubConfiguration(Root, ConfigurationPath.Combine(_Path, relativePath));
            }
        }

        public IConfiguration this[string[] relativePaths]
        {
            get
            {
                if (relativePaths == null)
                    throw new ArgumentNullException(nameof(relativePaths));

                return new SubConfiguration(Root, ConfigurationPath.Combine(_Path, relativePaths));
            }
        }

        [NotNull]
        private JToken GetSubElement([NotNull] JObject root, string[] path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            JToken current = root;
            foreach (var element in path)
            {
                var child = current[element];
                if (child == null)
                    return new JObject();

                current = child;
            }

            return current;
        }

        private JToken GetElement()
        {
            var currentRootElement = Root.GetElement();
            if (ReferenceEquals(currentRootElement, _PreviousRootElement))
                return _PreviousElement;

            _PreviousRootElement = currentRootElement;
            _PreviousElement = GetSubElement(currentRootElement, _Path.Split('/'));
            
            return _PreviousElement;
        }

        public IConfigurationElement<T> Element<T>() => new ConfigurationElement<T>(GetElement);

        [NotNull]
        protected abstract RootConfiguration Root { get; }
    }
}