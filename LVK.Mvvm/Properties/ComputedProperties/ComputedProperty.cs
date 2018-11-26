using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Mvvm.Scopes;

namespace LVK.Mvvm.Properties.ComputedProperties
{
    internal class ComputedProperty<T> : MvvmBound, IReadableProperty<T>, IDisposable, IPropertyWriteListener
    {
        [NotNull]
        private readonly Func<T> _GetComputedValue;

        [NotNull, ItemNotNull]
        private readonly HashSet<IProperty> _Dependencies = new HashSet<IProperty>(new ObjectReferenceEqualityComparer()); 

        private bool _MustRecompute = true;
        
        private T _Value;
        
        public ComputedProperty([NotNull] IMvvmContext context, [NotNull] Func<T> getComputedValue)
            : base(context)
        {
            _GetComputedValue = getComputedValue ?? throw new ArgumentNullException(nameof(getComputedValue));

            Recompute();
        }

        public T Value
        {
            get
            {
                if (_MustRecompute)
                    Recompute();
            
                return _Value;
            }
        }

        private void Recompute()
        {
            var scope = new PropertyReadScope();
            using (Context.ReadScope(scope))
            {
                _Value = _GetComputedValue();
                _MustRecompute = false;
            }

            UnregisterWriteListener();
            _Dependencies.Clear();
            _Dependencies.AddRange(scope.GetReadProperties());
            RegisterWriteListener();
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

        public T PeekValue => _Value;

        public void Dispose()
        {
            UnregisterWriteListener();
        }

        public void RegisterWrite(IProperty property)
        {
            if (_Dependencies.Contains(property) && !_MustRecompute)
            {
                Context.RegisterWrite(this);
                
                _MustRecompute = true;
                _Value = default;
            }
        }
    }
}