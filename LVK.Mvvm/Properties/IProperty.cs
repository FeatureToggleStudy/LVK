using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    public interface IProperty
    {
    }

    [PublicAPI]
    public interface IProperty<T> : IReadableProperty<T>
    {
        new T Value { get; set; }
    }
}