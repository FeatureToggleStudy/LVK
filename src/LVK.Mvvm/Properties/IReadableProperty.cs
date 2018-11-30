using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    [PublicAPI]
    public interface IReadableProperty<out T> : IProperty
    {
        [CanBeNull]
        T Value { get; }

        [CanBeNull]
        T PeekValue { get; }
    }
}