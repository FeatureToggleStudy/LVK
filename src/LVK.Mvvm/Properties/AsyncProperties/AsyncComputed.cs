using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Mvvm.Scopes;

namespace LVK.Mvvm.Properties.AsyncProperties
{
    internal class AsyncComputed<T> : Property<T>, IDisposable, IPropertyWriteListener
    {
        [NotNull]
        private readonly Func<CancellationToken, Task<T>> _CreateComputeTask;

        [CanBeNull]
        private CancellationTokenSource _CancellationTokenSource;

        [NotNull, ItemNotNull]
        private readonly HashSet<IProperty> _Dependencies = new HashSet<IProperty>(new ObjectReferenceEqualityComparer()); 
        
        public AsyncComputed([NotNull] IMvvmContext context, [NotNull] Func<CancellationToken, Task<T>> createComputeTask)
            : base(context, default)
        {
            _CreateComputeTask = createComputeTask ?? throw new ArgumentNullException(nameof(createComputeTask));

            Recompute();
        }

        private async void Recompute()
        {
            var cts = Interlocked.Exchange(ref _CancellationTokenSource, null);
            try
            {
                cts?.Cancel();
                cts?.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }

            await Task.Yield();
            var cancellationTokenSource = _CancellationTokenSource = new CancellationTokenSource();

            Value = default;
            var scope = new PropertyReadScope();
            Task<T> task;
            using (Context.ReadScope(scope))
            {
                task = _CreateComputeTask(_CancellationTokenSource.Token);
            }

            if (!_CancellationTokenSource.IsCancellationRequested)
            {
                UnregisterWriteListener();
                _Dependencies.Clear();
                _Dependencies.AddRange(scope.GetReadProperties());
                RegisterWriteListener();
            }

            if (task != null)
            {
                try
                {
                    Value = await task;
                }
                catch (TaskCanceledException)
                {
                    // Do nothing
                }

                Interlocked.CompareExchange(ref _CancellationTokenSource, null, cancellationTokenSource);
                cancellationTokenSource.Dispose();
            }
        }
 
        public void Dispose()
        {
            UnregisterWriteListener();
        }
        
        private void RegisterWriteListener()
        {
            foreach (var property in _Dependencies)
                Context.RegisterWriteListener(property, this);
        }

        private void UnregisterWriteListener()
        {
            foreach (var property in _Dependencies)
                Context.UnregisterWriteListener(property, this);
        }

        public void RegisterWrite(IProperty property)
        {
            if (_Dependencies.Contains(property))
            {
                Context.RegisterWrite(this);

                Recompute();
            }
        }
    }
}