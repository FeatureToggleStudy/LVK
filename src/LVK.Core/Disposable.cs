using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public abstract class Disposable : IDisposable
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<IDisposable> _Disposables = new HashSet<IDisposable>(new ObjectReferenceEqualityComparer());

        [NotNull]
        private readonly object _Lock = new object();

        private volatile bool _IsDisposed;

        [CanBeNull]
        protected T RegisterDisposable<T>([CanBeNull] T instance)
            where T: class, IDisposable
        {
            if (instance is null)
                return null;

            AssertNotDisposed();

            lock (_Lock)
                _Disposables.Add(instance);

            return instance;
        }

        protected void UnregisterDisposable([CanBeNull] IDisposable instance)
        {
            if (instance != null)
            {
                AssertNotDisposed();
                lock (_Lock)
                    _Disposables.Remove(instance);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            List<IDisposable> disposables;

            lock (_Lock)
            {
                disposables = _Disposables.ToList();
                _Disposables.Clear();
                _IsDisposed = true;
            }

            foreach (var disposable in disposables)
                disposable.Dispose();
        }

        protected void AssertNotDisposed()
        {
            if (_IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose() => Dispose(true);
    }
}