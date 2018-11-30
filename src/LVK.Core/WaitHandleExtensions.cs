using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class WaitHandleExtensions
    {
        [NotNull]
        public static Task AsTask([NotNull] this WaitHandle handle) => AsTask(handle, Timeout.InfiniteTimeSpan);

        [NotNull]
        public static Task AsTask([NotNull] this WaitHandle handle, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<object>();
            RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(
                handle, (state, timedOut) =>
                {
                    TaskCompletionSource<object> localTcs = ((TaskCompletionSource<object>)state).NotNull();
                    if (timedOut)
                        localTcs.TrySetCanceled();
                    else
                        localTcs.TrySetResult(null);
                }, tcs, timeout, executeOnlyOnce: true);

            tcs.Task.NotNull().ContinueWith((_, state) => ((RegisteredWaitHandle)state).NotNull().Unregister(null), registration, TaskScheduler.Default);
            return tcs.Task;
        }
    }
}