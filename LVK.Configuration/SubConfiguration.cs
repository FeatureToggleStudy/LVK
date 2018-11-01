using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Configuration
{
    internal class SubConfiguration : BaseConfiguration
    {
        public SubConfiguration([NotNull] RootConfiguration root, [NotNull] string path, [NotNull] JsonSerializer serializer)
            : base(path, serializer)
        {
            Root = root;
        }

        protected override RootConfiguration Root { get; }
    }
}