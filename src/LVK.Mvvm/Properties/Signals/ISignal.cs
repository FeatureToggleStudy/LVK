using JetBrains.Annotations;

namespace LVK.Mvvm.Properties.Signals
{
    [PublicAPI]
    public interface ISignal
    {
        void Pulse();
        void RegisterDependency();
    }
}