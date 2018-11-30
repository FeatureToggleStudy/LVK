using System;
using System.Windows.Input;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;
using LVK.Mvvm.Wpf.Properties;

namespace LVK.Mvvm.Wpf.ViewModels
{
    public partial class WpfViewModel
    {
        [NotNull]
        protected ICommand WpfCommand([NotNull] string name, [NotNull] Action executeAction)
            => WpfCommand(name, Property(true), executeAction);

        [NotNull]
        protected ICommand WpfCommand([NotNull] string name, [NotNull] Func<bool> canExecuteExpression, [NotNull] Action executeAction)
            => WpfCommand(name, Computed(canExecuteExpression), executeAction);

        [NotNull]
        protected ICommand WpfCommand(
            [NotNull] string name, [CanBeNull] IReadableProperty<bool> canExecuteProperty, [NotNull] Action executeAction)
        {
            if (executeAction == null)
                throw new ArgumentNullException(nameof(executeAction));

            return new WpfCommand(Context, canExecuteProperty, executeAction);
        }
    }
}