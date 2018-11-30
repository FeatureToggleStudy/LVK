using JetBrains.Annotations;

namespace LVK.Mvvm.Properties.Commands
{
    [PublicAPI]
    public interface ICommand
    {
        bool CanExecute();
        void Execute();
    }
}