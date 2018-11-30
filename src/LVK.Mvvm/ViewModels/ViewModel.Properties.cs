using JetBrains.Annotations;

using LVK.Mvvm.Properties;

namespace LVK.Mvvm.ViewModels
{
    public abstract partial class ViewModel
    {
        [NotNull]
        protected IProperty<T> Property<T>([NotNull] string name, T defaultValue)
            => RegisterProperty(new Property<T>(Context, defaultValue), name);

        [NotNull]
        protected IProperty<T> Property<T>(T defaultValue) => RegisterProperty(new Property<T>(Context, defaultValue), null);
    }
}