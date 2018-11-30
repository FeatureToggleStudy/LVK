using System;

using JetBrains.Annotations;

namespace LVK.Mvvm.Properties.Commands
{
    internal class Command : MvvmBound, ICommand
    {
        [NotNull]
        private readonly IReadableProperty<bool> _CanExecuteProperty;

        [NotNull]
        private readonly Action _ExecuteAction;

        public Command([NotNull] IMvvmContext context, [NotNull] IReadableProperty<bool> canExecuteProperty, [NotNull] Action executeAction)
            : base(context)
        {
            _CanExecuteProperty = canExecuteProperty ?? throw new ArgumentNullException(nameof(canExecuteProperty));
            _ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
        }

        public bool CanExecute() => _CanExecuteProperty.Value;
        public void Execute() => _ExecuteAction();
    }
}