using System;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;
using LVK.Mvvm.Scopes;

namespace LVK.Mvvm
{
    [PublicAPI]
    public interface IMvvmContext
    {
        void RegisterRead([NotNull] IProperty property);
        void RegisterWrite([NotNull] IProperty property);

        [NotNull]
        IDisposable ReadScope([NotNull] IPropertyReadScope scope);

        void RegisterWriteListener([NotNull] IProperty property, [NotNull] IPropertyWriteListener propertyWriteListener);
        void UnregisterWriteListener([NotNull] IProperty property, [NotNull] IPropertyWriteListener propertyWriteListener);
        
        event EventHandler PropertyWriteScopeEntered;
        event EventHandler PropertyWriteScopeExited;
    }
}