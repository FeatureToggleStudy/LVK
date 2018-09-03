using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IApplicationInitialization
    {
        [NotNull]
        Task Initialize(CancellationToken cancellationToken);
    }
}