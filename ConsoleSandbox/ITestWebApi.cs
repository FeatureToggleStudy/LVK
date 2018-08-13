using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace ConsoleSandbox
{
    public interface ITestWebApi
    {
        [NotNull, ItemNotNull]
        Task<string[]> GetValuesAsync(CancellationToken cancellationToken);

        [NotNull, ItemNotNull]
        Task<string> GetValueAsync(int id, CancellationToken cancellationToken);
        
        [NotNull]
        Task PostAsync([NotNull] string value, CancellationToken cancellationToken);

        [NotNull]
        Task PutAsync(int id, [NotNull] string value, CancellationToken cancellationToken);
        
        [NotNull]
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}