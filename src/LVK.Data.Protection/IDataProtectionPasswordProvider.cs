using JetBrains.Annotations;

namespace LVK.Data.Protection
{
    [PublicAPI]
    public interface IDataProtectionPasswordProvider
    {
        [CanBeNull]
        string TryGetPassword([NotNull] string passwordName);
    }
}