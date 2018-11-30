using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Mvvm.Properties;
using LVK.Mvvm.Scopes;

namespace LVK.Mvvm
{
    internal class MvvmContext : IMvvmContext
    {
        [NotNull, ItemNotNull]
        private readonly Stack<IPropertyReadScope> _ReadScopes = new Stack<IPropertyReadScope>();

        private int _WriteScopeLevel;

        [NotNull]
        private readonly Dictionary<IProperty, HashSet<IPropertyWriteListener>> _PropertyWriteListeners =
            new Dictionary<IProperty, HashSet<IPropertyWriteListener>>(new ObjectReferenceEqualityComparer());
        
        public void RegisterRead(IProperty property)
        {
            _ReadScopes.PeekOrDefault()?.RegisterRead(property);
        }

        public void RegisterWrite(IProperty property)
        {
            using (WriteScope())
            {
                if (!_PropertyWriteListeners.TryGetValue(property, out HashSet<IPropertyWriteListener> listeners))
                    return;

                foreach (var listener in listeners.ToList())
                    listener.RegisterWrite(property);
            }
        }

        public IDisposable WriteScope()
        {
            return new ActionDisposable(
                () =>
                {
                    if (++_WriteScopeLevel == 1)
                        PropertyWriteScopeEntered?.Invoke(this, EventArgs.Empty);
                }, () =>
                {
                    if (--_WriteScopeLevel == 0)
                        PropertyWriteScopeExited?.Invoke(this, EventArgs.Empty);
                });
        }

        public IDisposable ReadScope(IPropertyReadScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return new ActionDisposable(() => _ReadScopes.Push(scope), () => _ReadScopes.Pop());
        }

        public void RegisterWriteListener(IProperty property, IPropertyWriteListener propertyWriteListener)
        {
            _PropertyWriteListeners.GetOrAdd(property, () => new HashSet<IPropertyWriteListener>(new ObjectReferenceEqualityComparer()))
               .NotNull()
               .Add(propertyWriteListener);
        }

        public void UnregisterWriteListener(IProperty property, IPropertyWriteListener propertyWriteListener)
        {
            if (!_PropertyWriteListeners.TryGetValue(property, out HashSet<IPropertyWriteListener> listeners))
                return;

            if (!listeners.Remove(propertyWriteListener))
                return;

            if (listeners.Count == 0)
                _PropertyWriteListeners.Remove(property);
        }

        public event EventHandler PropertyWriteScopeEntered;

        public event EventHandler PropertyWriteScopeExited;
    }
}