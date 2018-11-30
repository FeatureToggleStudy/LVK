using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    [PublicAPI]
    public interface IPropertyWriteListener
    {
        void RegisterWrite([NotNull] IProperty property);
    }
}