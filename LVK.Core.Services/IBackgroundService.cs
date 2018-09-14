using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IBackgroundService
    {
        [NotNull]
        Task Execute(CancellationToken cancellationToken);
    }
}