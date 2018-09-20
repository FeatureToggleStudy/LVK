using System.Text;

using JetBrains.Annotations;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class OptionalJsonFileConfigurationLayer : BaseJsonFileConfigurationLayer
    {
        public OptionalJsonFileConfigurationLayer(
            [NotNull] IClock clock, [NotNull] string filename, [NotNull] Encoding encoding)
            : base(clock, filename, encoding)
        {
        }

        protected override void FileDoesNotExist()
        {
        }
    }
}