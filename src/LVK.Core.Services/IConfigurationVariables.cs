using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IConfigurationVariables
    {
        [NotNull]
        string Prefix { get; }

        (bool success, string value) TryGetValue([NotNull] string name);
    }
}