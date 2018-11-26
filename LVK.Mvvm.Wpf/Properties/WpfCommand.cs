using System;
using System.Diagnostics;
using System.Windows.Input;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;

namespace LVK.Mvvm.Wpf.Properties
{
    internal class WpfCommand : ICommand, IProperty, IPropertyWriteListener, IDisposable
    {
        [NotNull]
        private readonly IMvvmContext _Context;

        [CanBeNull]
        private readonly IReadableProperty<bool> _CanExecuteProperty;

        [NotNull]
        private readonly Action _ExecuteAction;

        public WpfCommand(
            [NotNull] IMvvmContext context, [CanBeNull] IReadableProperty<bool> canExecuteProperty, [NotNull] Action executeAction)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
            _CanExecuteProperty = canExecuteProperty;
            _ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));

            if (_CanExecuteProperty != null)
                context.RegisterWriteListener(_CanExecuteProperty, this);
        }

        public bool CanExecute(object parameter)
        {
            var result = _CanExecuteProperty?.Value ?? true;
            Debug.WriteLine($"Command.CanExecute returns {result}");
            return result;
        }

        public void Execute(object parameter) => _ExecuteAction();

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RegisterWrite(IProperty property)
        {
            if (property == _CanExecuteProperty)
            {
                Debug.WriteLine("CanExecuteChanged");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public void Dispose()
        {
            if (_CanExecuteProperty != null)
                _Context.UnregisterWriteListener(_CanExecuteProperty, this);
        }
    }
}