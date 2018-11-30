using JetBrains.Annotations;

using LVK.Mvvm.ViewModels;

namespace LVK.Mvvm.Wpf.ViewModels
{
    public abstract partial class WpfViewModel : ViewModel
    {
        protected WpfViewModel([NotNull] IMvvmContext context)
            : base(context)
        {
        }
    }
}