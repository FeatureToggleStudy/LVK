using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    [PublicAPI]
    public interface IApplicationCommand
    {
        [NotNull, ItemNotNull]
        string[] CommandNames { get; }

        [NotNull]
        string Description { get; }

        [NotNull]
        Task<int> TryExecute();
    }
}