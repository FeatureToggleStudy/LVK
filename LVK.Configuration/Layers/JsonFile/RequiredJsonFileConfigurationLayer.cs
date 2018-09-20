using System;
using System.Text;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class RequiredJsonFileConfigurationLayer : BaseJsonFileConfigurationLayer
    {
        public RequiredJsonFileConfigurationLayer(
            [NotNull] IClock clock, [NotNull] string filename, [NotNull] Encoding encoding)
            : base(clock, filename, encoding)
        {
        }

        protected override void FileDoesNotExist()
            => throw new InvalidOperationException($"configuration file '{Filename}' does not exist");
    }
}