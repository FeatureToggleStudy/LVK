using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Commands
{
    [PublicAPI]
    public interface ICommandDispatcher
    {
        [NotNull]
        Task<(bool success, TOutput output)> TryDispatch<TInput, TOutput>([NotNull] TInput input);

        [NotNull]
        Task<bool> TryDispatch<TInput>([NotNull] TInput input);
    }
}