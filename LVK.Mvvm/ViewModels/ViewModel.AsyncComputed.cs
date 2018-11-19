using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;
using LVK.Mvvm.Properties.AsyncProperties;

namespace LVK.Mvvm.ViewModels
{
    public abstract partial class ViewModel
    {
        [NotNull]
        protected IReadableProperty<T> AsyncComputed<T>([NotNull] Func<CancellationToken, Task<T>> createComputeTask)
            => RegisterProperty(new AsyncComputed<T>(Context, createComputeTask), null);

        [NotNull]
        protected IReadableProperty<T> AsyncComputed<T>([NotNull] string name, [NotNull] Func<CancellationToken, Task<T>> createComputeTask)
            => RegisterProperty(new AsyncComputed<T>(Context, createComputeTask), name);
    }
}