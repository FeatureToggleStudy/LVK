using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace ConsoleSandbox
{
    public interface ITestWebApi
    {
        [NotNull, ItemNotNull]
        Task<string[]> GetValuesAsync(CancellationToken cancellationToken);
    }
}