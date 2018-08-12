using System;
using System.Threading;

using JetBrains.Annotations;

namespace LVK.Core
{
    public class ActionDisposable : IDisposable
    {
        [CanBeNull]
        private Action _DisposeAction;

        public ActionDisposable([NotNull] Action disposeAction)
            : this(null, disposeAction)
        {
        }

        public ActionDisposable([CanBeNull] Action initAction, [NotNull] Action disposeAction)
        {
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));
            
            initAction?.Invoke();
            _DisposeAction = disposeAction;
        }

        public void Dispose() => Interlocked.Exchange(ref _DisposeAction, null)?.Invoke();
    }
}