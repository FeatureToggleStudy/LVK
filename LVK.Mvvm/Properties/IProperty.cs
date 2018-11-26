using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    [PublicAPI]
    public interface IProperty
    {
    }

    [PublicAPI]
    public interface IProperty<T> : IReadableProperty<T>
    {
        new T Value { get; set; }
    }
}