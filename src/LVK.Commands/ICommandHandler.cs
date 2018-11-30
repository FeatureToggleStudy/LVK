using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Commands
{
    [PublicAPI]
    public interface ICommandHandler<in TInput, TOutput>
    {
        [NotNull]
        Task<(bool success, TOutput output)> Handle([NotNull] TInput input);
    }

    [PublicAPI]
    public interface ICommandHandler<in TInput>
    {
        [NotNull]
        Task<bool> Handle([NotNull] TInput input);
    }
}