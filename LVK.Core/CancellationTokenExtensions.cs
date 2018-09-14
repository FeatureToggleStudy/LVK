using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class CancellationTokenExtensions
    {
        [NotNull]
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(() => tcs.SetResult(true));
            return tcs.Task.NotNull();
        }
    }
}