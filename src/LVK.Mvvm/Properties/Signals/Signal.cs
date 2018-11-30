using JetBrains.Annotations;

namespace LVK.Mvvm.Properties.Signals
{
    internal class Signal : MvvmBound, IProperty, ISignal
    {
        public Signal([NotNull] IMvvmContext context)
            : base(context)
        {
        }

        public void Pulse() => Context.RegisterWrite(this);

        public void RegisterDependency() => Context.RegisterRead(this);
    }
}