using JetBrains.Annotations;

using LVK.Mvvm.Properties.Signals;

namespace LVK.Mvvm.ViewModels
{
    public abstract partial class ViewModel
    {
        [NotNull]
        protected ISignal Signal() => RegisterProperty(new Signal(Context), null);

        [NotNull]
        protected ISignal Signal([NotNull] string name) => RegisterProperty(new Signal(Context), name);
    }
}