using JetBrains.Annotations;

namespace LVK.AppCore
{
    [PublicAPI]
    public interface IApplicationDataFolder
    {
        [CanBeNull]
        string FolderPath { get; }

        [NotNull]
        string GetDataFilePath([NotNull] string filename);
    }
}