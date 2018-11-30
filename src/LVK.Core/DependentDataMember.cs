using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Core
{
    internal class DependentDataMember
    {
        private readonly DependentDataKey _Key;

        [NotNull]
        private readonly DependentDataDependencyTracker _DependencyTracker;

        private bool _IsFirstSet;

        [CanBeNull]
        private Func<object> _ValueGetter;

        private bool _IsCached;

        [CanBeNull]
        private object _Value;

        public DependentDataMember(DependentDataKey key, [NotNull] DependentDataDependencyTracker dependencyTracker)
        {
            _Key = key;
            _DependencyTracker = dependencyTracker ?? throw new ArgumentNullException(nameof(dependencyTracker));
        }

        [CanBeNull]
        public T Get<T>()
        {
            _DependencyTracker.RegisterRead(_Key);
            Evaluate();
            return (T)_Value;
        }

        private void Evaluate()
        {
            if (_IsCached)
                return;

            assume(_ValueGetter != null);

            try
            {
                _DependencyTracker.BeginReadScope();
                _Value = _ValueGetter();
            }
            finally
            {
                _DependencyTracker.EndReadScopeFor(_Key);
            }

            _IsCached = true;
        }

        public void Set<T>([CanBeNull] T value)
        {
            if (_ValueGetter == null && !_IsFirstSet)
            {
                if (value == null && _Value == null)
                    return;

                if (_Value != null && EqualityComparer<T>.Default.Equals(value, (T)_Value))
                    return;
            }

            _ValueGetter = null;
            _Value = value;
            _IsCached = true;
            _IsFirstSet = false;

            _DependencyTracker.RegisterWrite(_Key);
        }

        public void Set<T>([NotNull] Func<T> valueGetter)
        {
            _ValueGetter = () => valueGetter();
            _Value = null;
            _IsCached = false;
            _IsFirstSet = false;

            _DependencyTracker.RegisterWrite(_Key);
        }

        public void Invalidate()
        {
            if (_ValueGetter == null)
                return;

            _Value = null;
            _IsCached = false;

            _DependencyTracker.RegisterWrite(_Key);
        }
    }
}