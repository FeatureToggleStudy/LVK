using JetBrains.Annotations;

namespace LVK.Configuration
{
    internal class SubConfiguration : BaseConfiguration
    {
        public SubConfiguration([NotNull] RootConfiguration root, [NotNull] string path)
            : base(path)
        {
            Root = root;
        }

        protected override RootConfiguration Root { get; }
    }
}