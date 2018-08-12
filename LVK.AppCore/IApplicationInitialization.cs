using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    public interface IApplicationInitialization
    {
        [NotNull]
        Task Initialize(CancellationToken cancellationToken);
    }
}