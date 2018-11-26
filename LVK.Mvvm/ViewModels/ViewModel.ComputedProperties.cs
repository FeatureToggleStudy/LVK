using System;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;
using LVK.Mvvm.Properties.ComputedProperties;

namespace LVK.Mvvm.ViewModels
{
    public abstract partial class ViewModel
    {
        [NotNull]
        protected IReadableProperty<T> Computed<T>([NotNull] string name, [NotNull] Func<T> getComputedValue)
            => RegisterProperty(new ComputedProperty<T>(Context, getComputedValue), name);

        [NotNull]
        protected IReadableProperty<T> Computed<T>([NotNull] Func<T> getComputedValue)
            => RegisterProperty(new ComputedProperty<T>(Context, getComputedValue), null);
    }
}